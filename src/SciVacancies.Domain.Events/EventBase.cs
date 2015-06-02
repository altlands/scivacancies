﻿using System;

namespace SciVacancies.Domain.Events
{
    public abstract class EventBase
    {
        private Guid id { get; set; }
        private DateTime timeStamp { get; set; }

        public EventBase()
        {
            id = Guid.NewGuid();
            timeStamp = DateTime.UtcNow;
        }

        public Guid Id { get { return id; } }
        public DateTime TimeStamp { get { return timeStamp; } }
    }
}
