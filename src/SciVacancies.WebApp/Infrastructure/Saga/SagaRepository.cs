﻿using System;
using System.Collections.Generic;
using System.Linq;
using NEventStore;
using NEventStore.Persistence;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class SagaEventStoreRepository : ISagaRepository, IDisposable
    {
        private const string SagaTypeHeader = "SagaType";

        private const string UndispatchedMessageHeader = "UndispatchedMessage.";

        private readonly IStoreEvents _eventStore;

        private readonly IConstructSagas _factory;

        private readonly IDictionary<string, IEventStream> _streams = new Dictionary<string, IEventStream>();

        public SagaEventStoreRepository(IStoreEvents eventStore, IConstructSagas factory)
        {
            _eventStore = eventStore;
            _factory = factory;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public TSaga GetById<TSaga>(string bucketId, Guid sagaId) where TSaga : class, ISaga
        {
            return BuildSaga<TSaga>(sagaId, OpenStream(bucketId, sagaId));
        }

        public void Save(string bucketId, ISaga saga, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
        {
            if (saga == null)
            {
                throw new ArgumentNullException(nameof(saga));
            }

            Dictionary<string, object> headers = PrepareHeaders(saga, updateHeaders);
            IEventStream stream = PrepareStream(bucketId, saga, headers);

            Persist(stream, commitId);

            saga.ClearUncommittedEvents();
            saga.ClearUndispatchedMessages();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            lock (_streams)
            {
                foreach (var stream in _streams)
                {
                    stream.Value.Dispose();
                }

                _streams.Clear();
            }
        }

        private IEventStream OpenStream(string bucketId, Guid sagaId)
        {
            IEventStream stream;
            var sagaKey = bucketId + "+" + sagaId.ToString();
            if (_streams.TryGetValue(sagaKey, out stream))
            {
                return stream;
            }

            try
            {
                stream = _eventStore.OpenStream(bucketId, sagaId, 0, int.MaxValue);
            }
            catch (StreamNotFoundException)
            {
                stream = _eventStore.CreateStream(bucketId, sagaId);
            }
            //if (_streams[sagaKey] == null) throw new ArgumentNullException("Can't find the stream by sagaKey : {0}", sagaKey);
            if(!_streams.ContainsKey(sagaKey))
            {
                try
                {
                    _streams.Add(sagaKey, stream);
                }
                catch (Exception exception)
                {
                    if (!exception.Message.ToLower().Contains("contains") || !exception.Message.ToLower().Contains("already"))
                    {
                        throw exception;
                    }
                }
            }

            //return _streams[sagaKey] = stream;
            return _streams[sagaKey];
        }

        private TSaga BuildSaga<TSaga>(Guid sagaId, IEventStream stream) where TSaga : class, ISaga
        {
            var saga = (TSaga)_factory.Build(typeof(TSaga), sagaId);
            foreach (var @event in stream.CommittedEvents.Select(x => x.Body))
            {
                saga.Transition(@event);
            }

            saga.ClearUncommittedEvents();
            saga.ClearUndispatchedMessages();

            return saga;
        }

        private static Dictionary<string, object> PrepareHeaders(
            ISaga saga, Action<IDictionary<string, object>> updateHeaders)
        {
            var headers = new Dictionary<string, object>();

            headers[SagaTypeHeader] = saga.GetType().FullName;
            if (updateHeaders != null)
            {
                updateHeaders(headers);
            }

            int i = 0;
            foreach (var command in saga.GetUndispatchedMessages())
            {
                headers[UndispatchedMessageHeader + i++] = command;
            }

            return headers;
        }

        private IEventStream PrepareStream(string bucketId, ISaga saga, Dictionary<string, object> headers)
        {
            IEventStream stream;
            var sagaKey = bucketId + "+" + saga.Id.ToString();
            if (!_streams.TryGetValue(sagaKey, out stream))
            {
                _streams[sagaKey] = stream = _eventStore.CreateStream(bucketId, saga.Id);
            }

            foreach (var item in headers)
            {
                stream.UncommittedHeaders[item.Key] = item.Value;
            }

            saga.GetUncommittedEvents().Cast<object>().Select(x => new EventMessage { Body = x }).ToList().ForEach(stream.Add);

            return stream;
        }

        private static void Persist(IEventStream stream, Guid commitId)
        {
            try
            {
                stream.CommitChanges(commitId);
            }
            catch (DuplicateCommitException)
            {
                stream.ClearChanges();
            }
            catch (StorageException e)
            {
                throw new CommonDomain.Persistence.PersistenceException(e.Message, e);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
