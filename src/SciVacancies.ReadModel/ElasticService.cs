using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Nest;

namespace SciVacancies.ReadModel
{
    public class ElasticService : IElasticService
    {
        public string DefaultIndexName = "scivacancies";
        public ElasticClient Connect()
        {
            //var url = new Uri("http://localhost:9200");
            var url = new Uri("http://altlandev01.cloudapp.net:9200/");
            var config = new ConnectionSettings(url, defaultIndex: DefaultIndexName);
            return new ElasticClient(config);
        }
        public void CreateIndex()
        {
            Connect().CreateIndex(DefaultIndexName, c => c
                                .AddMapping<Vacancy>(am => am
                                    .MapFromAttributes()
                                )
                                .AddMapping<Organization>(am => am
                                    .MapFromAttributes()
                                )
                            );
        }
        public void RemoveIndex()
        {
            Connect().DeleteIndex(s => s.Index(DefaultIndexName));
        }
        public void RestoreIndexFromReadModel()
        {

        }
        public void IndexOrganization(Organization organization)
        {
            Connect().Index(organization);
        }
        public void UpdateOrganization(Organization organization)
        {
            Connect().Update<Organization>(u => u.IdFrom(organization).Doc(organization));
        }
        public void IndexVacancy(Vacancy vacancy)
        {
            Connect().Index(vacancy);
        }
        public void UpdateVacancy(Vacancy vacancy)
        {
            Connect().Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
        public ISearchResponse<Vacancy> Search(string query, int pageSize, int pageIndex, List<Guid> regions, List<Guid> foivs, List<Guid> universities, List<int> directions)
        {
            //                return Connect().Search<ElasticResource>(s => s.Index(DefaultName).Sort(sd => sd.OnField(of => of.Resource.Code.Suffix("raw")).NestedMax().Descending()).Skip((c.Page - 1) * ResPerPage).Take(ResPerPage).MinScore(MinScore).Query(QueryToggle(c, employeeNumber)));
            return Connect().Search<Vacancy>(s => s
                 .Index(DefaultIndexName)
                 .Skip((pageIndex - 1) * pageSize)
                 .Take(pageSize)
                 .Query(qr => qr
                     .FuzzyLikeThis(flt => flt
                         .LikeText(query)
                     )
                 //.Filtered(ftd => ftd
                 //    .Query(q => q
                 //        .FuzzyLikeThis(flt => flt.LikeText(query))
                 //    )
                 //    .Filter(f => f
                 //        .Bool(b=>b
                 //            //.Must(mst=>mst
                 //            //    .Terms().
                 //            //)
                 //            //&& b.Must(mst=>mst)
                 //        )                        
                 //    )
                 //)
                 )
             );
        }

        //    Connect().CreateIndex(DefaultName, c => c
        //        .Analysis(an => an
        //            .Analyzers(ans => ans.Add("loweranalyzer", lowerAn)))
        //        .AddMapping<ElasticResource>(am => am
        //            .MapFromAttributes()
        //            .Properties(pr => pr
        //                .NestedObject<Resource>(no => no
        //                    .Name(p => p.Resource)
        //                    .MapFromAttributes()
        //                    .Properties(pp => pp
        //                        .MultiField(mf => mf
        //                            .Name(r => r.Code).Path(MultiFieldMappingPath.Full)
        //                            .Fields(fd => fd
        //                                .String(ps => ps.Name(nn => nn.Code.Suffix("raw")).SearchAnalyzer("loweranalyzer").IndexAnalyzer("loweranalyzer").Boost(0.22))
        //                                .String(ps => ps.Name(nn => nn.Code).Index(FieldIndexOption.Analyzed).Boost(0.22))
        //                            ))
        //                        .MultiField(mf => mf
        //                            .Name(r => r.Name).Path(MultiFieldMappingPath.Full)
        //                            .Fields(fd => fd
        //                                .String(ps => ps.Name(nn => nn.Name.Suffix("raw")).SearchAnalyzer("loweranalyzer").IndexAnalyzer("loweranalyzer").Boost(0.21))
        //                                .String(ps => ps.Name(nn => nn.Name).Index(FieldIndexOption.Analyzed).Boost(0.21))
        //                            ))
        //                        .MultiField(mf => mf
        //                            .Name(r => r.Owner).Path(MultiFieldMappingPath.Full)
        //                            .Fields(fd => fd
        //                                .String(ps => ps.Name(nn => nn.Owner.Suffix("raw")).SearchAnalyzer("loweranalyzer").IndexAnalyzer("loweranalyzer").Boost(0.09))
        //                                .String(ps => ps.Name(nn => nn.Owner).Index(FieldIndexOption.Analyzed).Boost(0.09))
        //                            ))
        //                ))
        //                .NestedObject<Role>(no => no
        //                    .Name("roles")
        //                    .MapFromAttributes()
        //                    .Properties(pp => pp
        //                        .MultiField(mf => mf
        //                            .Name(r => r.Name).Path(MultiFieldMappingPath.Full)
        //                            .Fields(fd => fd
        //                                .String(ps => ps.Name(nn => nn.Name.Suffix("raw")).SearchAnalyzer("loweranalyzer").IndexAnalyzer("loweranalyzer").Boost(0.02))
        //                                .String(ps => ps.Name(nn => nn.Name).Index(FieldIndexOption.Analyzed).Boost(0.02))
        //                        ))
        //                        .MultiField(mf => mf
        //                            .Name(r => r.Description).Path(MultiFieldMappingPath.Full)
        //                            .Fields(fd => fd
        //                                .String(ps => ps.Name(nn => nn.Description.Suffix("raw")).SearchAnalyzer("loweranalyzer").IndexAnalyzer("loweranalyzer").Boost(0.01))
        //                                .String(ps => ps.Name(nn => nn.Description).Index(FieldIndexOption.Analyzed).Boost(0.01))
        //                        ))
        //                ))
        //                            )));
        //    CatalogEvents.Log.TextPoint("Elastic Index creation is finished");
        //    //DateTime finishTime = DateTime.Now;
        //    //Console.WriteLine(" - OK ( " + (finishTime - startTime) + " )");
        //}
        //public void DeleteIndex()
        //{
        //    //DateTime startTime = DateTime.Now;
        //    //Console.Write("Removing Index");
        //    CatalogEvents.Log.TextPoint("Elastic Index deleting...");

        //    Connect().DeleteIndex(s => s.Index(DefaultName));

        //    CatalogEvents.Log.TextPoint("Elastic Index deleting is finished");
        //    //DateTime finishTime = DateTime.Now;
        //    //Console.WriteLine(" - OK ( " + (finishTime - startTime) + " )");
        //}
        //public void FillIndex()
        //{
        //    //DateTime startTime = DateTime.Now;
        //    //Console.Write("Filling Index with data ");

        //    //int cursorLeft = Console.CursorLeft;
        //    //int cursorTop = Console.CursorTop;
        //    CatalogEvents.Log.TextPoint("Elastic Index filling...");

        //    List<ElasticResource> elasticResources = new List<ElasticResource>();
        //    List<Resource> resources = new List<Resource>();


        //    int i = 0;
        //    int n = 100;

        //    bool temp = false;
        //    BulkDescriptor descriptor;

        //    using (var _catalogDb = new CatalogDb())
        //    {
        //        temp = _catalogDb.Resources.OrderBy(o => o.UNID).Skip(n * i).Take(1).Any();
        //    }

        //    while (temp)
        //    {
        //        descriptor = new BulkDescriptor();

        //        using (var _catalogDb = new CatalogDb())
        //        {
        //            _catalogDb.Resources.OrderBy(o => o.UNID).Skip(n * i).Take(n).ToList().ForEach(r => descriptor
        //                .Index<ElasticResource>(op => op.Document(new ElasticResource()
        //                {
        //                    Resource = r,
        //                    Tags = r.Tags.Select(s => s.Name).ToList(),
        //                    Categories = r.Categories.Select(s => s.CorePath).ToList(),
        //                    Roles = r.Roles.ToList()
        //                })));

        //            i++;
        //            temp = _catalogDb.Resources.OrderBy(o => o.UNID).Skip(n * i).Take(1).Any();

        //        }
        //        CatalogEvents.Log.TextPoint("Connecting descriptor to ElasticDb");

        //        Connect().Bulk(descriptor);

        //        CatalogEvents.Log.TextPoint("Descriptor connected");

        //        descriptor = null;

        //        if ((i * n) % 1000 == 0)
        //        {
        //            CatalogEvents.Log.TransferedResources(i * n);
        //        }

        //        //Console.SetCursorPosition(cursorLeft, cursorTop);
        //        //Console.Write(i * n);
        //    }
        //    //DateTime finishTime = DateTime.Now;
        //    //Console.WriteLine(" - OK ( " + (finishTime - startTime) + " )");
        //    CatalogEvents.Log.TextPoint("Elastic Index filling finished");
        //}
        //public SearchData FuzzySearch(SearchContainer c, string employeeNumber)
        //{
        //    var result = Search(c, employeeNumber);

        //    long total = result.Total;
        //    int pages = 0;
        //    int maxI = (int)Math.Ceiling((double)total / (double)ResPerPage);
        //    for (int i = 1; i <= maxI; i++)
        //    {
        //        pages++;
        //    }
        //    if ((total / ResPerPage) < 1)
        //    {
        //        pages = 1;
        //    }
        //    c.Total = total;
        //    return new SearchData() { Results = result.Documents.ToList(), Pages = pages, SearchContainer = c };
        //}
        //public ISearchResponse<ElasticResource> Search(SearchContainer c, string employeeNumber)
        //{
        //    if (c.Extended)
        //    {
        //        if (c.OrderType == 1)
        //        {
        //            if (!c.SortAsc)
        //            {
        //                return Connect().Search<ElasticResource>(s => s.Index(DefaultName).Sort(sd => sd.OnField(of => of.Resource.Code.Suffix("raw")).NestedMax().Descending()).Skip((c.Page - 1) * ResPerPage).Take(ResPerPage).MinScore(MinScore).Query(QueryToggle(c, employeeNumber)));
        //            }
        //            else
        //            {
        //                return Connect().Search<ElasticResource>(s => s.Index(DefaultName).Sort(sd => sd.OnField(of => of.Resource.Code.Suffix("raw")).NestedMax().Ascending()).Skip((c.Page - 1) * ResPerPage).Take(ResPerPage).MinScore(MinScore).Query(QueryToggle(c, employeeNumber)));
        //            }
        //        }
        //        else
        //        {
        //            return Connect().Search<ElasticResource>(s => s.Index(DefaultName).Skip((c.Page - 1) * ResPerPage).Take(ResPerPage).MinScore(MinScore).Query(QueryToggle(c, employeeNumber)));
        //        }
        //    }
        //    else
        //    {
        //        if (c.OrderType == 1)
        //        {
        //            if (!c.SortAsc)
        //            {
        //                var matchResults = Connect().Search<ElasticResource>(s => s.Index(DefaultName).Sort(sd => sd.OnField(of => of.Resource.Code.Suffix("raw")).NestedMax().Descending())
        //                    .Skip((c.Page - 1) * ResPerPage)
        //                    .Take(ResPerPage)
        //                    .MinScore(MinScore)
        //                    .Query(q => q
        //                        .Bool(bb => bb
        //                            .Should(shh => shh
        //                                .Nested(ne => ne
        //                                    .Path(pa => pa.Resource)
        //                                    .Query(qr => qr
        //                                    .Bool(b => b
        //                                        .Should(sh => sh
        //                                            .MatchPhrasePrefix(m => m.OnField(of => of.Resource.Code.Suffix("raw")).Query(c.Query))
        //                                            || sh.Match(m => m.OnField(of => of.Resource.Name).Query(c.Query))
        //                                            || sh.MatchPhrasePrefix(p2 => p2.OnField(o2 => o2.Resource.Owner.Suffix("raw")).Query(c.Query))
        //                                            ))
        //                                ))
        //                                || shh.Match(m => m.OnField(of => of.Tags).Query(c.Query))
        //                                || shh.Nested(ne => ne
        //                                    .Path(pa => pa.Roles)
        //                                    .Query(qr => qr
        //                                    .Bool(b => b
        //                                        .Should(sh => sh
        //                                            .MatchPhrasePrefix(m => m.OnField(of => of.Roles.FirstOrDefault().Name.Suffix("raw")).Query(c.Query))
        //                                            || sh.MatchPhrasePrefix(m => m.OnField(of => of.Roles.FirstOrDefault().Description.Suffix("raw")).Query(c.Query))
        //                                            ))
        //                                    ))
        //                                ))
        //                            )
        //                        );
        //                if (matchResults.Documents.Count() > 0)
        //                {
        //                    return matchResults;
        //                }
        //                else
        //                {
        //                    return Connect().Search<ElasticResource>(s => s.Index(DefaultName).Sort(sd => sd.OnField(of => of.Resource.Code.Suffix("raw")).NestedMax().Descending())
        //                        .Skip((c.Page - 1) * ResPerPage)
        //                        .Take(ResPerPage)
        //                        .MinScore(MinScore)
        //                        .Query(q => q.FuzzyLikeThis(flt => flt.LikeText(c.Query))));
        //                }
        //            }
        //            else
        //            {
        //                var matchResults = Connect().Search<ElasticResource>(s => s.Index(DefaultName).Sort(sd => sd.OnField(of => of.Resource.Code.Suffix("raw")).NestedMax().Ascending())
        //                    .Skip((c.Page - 1) * ResPerPage)
        //                    .Take(ResPerPage)
        //                    .MinScore(MinScore)
        //                    .Query(q => q
        //                        .Bool(bb => bb
        //                            .Should(shh => shh
        //                                .Nested(ne => ne
        //                                    .Path(pa => pa.Resource)
        //                                    .Query(qr => qr
        //                                    .Bool(b => b
        //                                        .Should(sh => sh
        //                                            .MatchPhrasePrefix(m => m.OnField(of => of.Resource.Code.Suffix("raw")).Query(c.Query))
        //                                            || sh.Match(m => m.OnField(of => of.Resource.Name).Query(c.Query))
        //                                            || sh.MatchPhrasePrefix(p2 => p2.OnField(o2 => o2.Resource.Owner.Suffix("raw")).Query(c.Query))
        //                                            ))
        //                                ))
        //                                || shh.Match(m => m.OnField(of => of.Tags).Query(c.Query))
        //                                || shh.Nested(ne => ne
        //                                    .Path(pa => pa.Roles)
        //                                    .Query(qr => qr
        //                                    .Bool(b => b
        //                                        .Should(sh => sh
        //                                            .MatchPhrasePrefix(m => m.OnField(of => of.Roles.FirstOrDefault().Name.Suffix("raw")).Query(c.Query))
        //                                            || sh.MatchPhrasePrefix(m => m.OnField(of => of.Roles.FirstOrDefault().Description.Suffix("raw")).Query(c.Query))
        //                                            ))
        //                                    ))
        //                                ))
        //                            )
        //                        );
        //                if (matchResults.Documents.Count() > 0)
        //                {
        //                    return matchResults;
        //                }
        //                else
        //                {
        //                    return Connect().Search<ElasticResource>(s => s.Index(DefaultName).Sort(sd => sd.OnField(of => of.Resource.Code.Suffix("raw")).NestedMax().Ascending())
        //                        .Skip((c.Page - 1) * ResPerPage)
        //                        .Take(ResPerPage)
        //                        .MinScore(MinScore)
        //                        .Query(q => q.FuzzyLikeThis(flt => flt.LikeText(c.Query))));
        //                }
        //            }
        //        }
        //        else
        //        {
        //            var matchResults = Connect().Search<ElasticResource>(s => s.Index(DefaultName)
        //                    .Skip((c.Page - 1) * ResPerPage)
        //                    .Take(ResPerPage)
        //                    .MinScore(MinScore)
        //                    .Query(q => q
        //                        .Bool(bb => bb
        //                            .Should(shh => shh
        //                                .Nested(ne => ne
        //                                    .Path(pa => pa.Resource)
        //                                    .Query(qr => qr
        //                                    .Bool(b => b
        //                                        .Should(sh => sh
        //                                            .MatchPhrasePrefix(m => m.OnField(of => of.Resource.Code.Suffix("raw")).Query(c.Query))
        //                                            || sh.Match(m => m.OnField(of => of.Resource.Name).Query(c.Query))
        //                                            || sh.MatchPhrasePrefix(p2 => p2.OnField(o2 => o2.Resource.Owner.Suffix("raw")).Query(c.Query))
        //                                            ))
        //                                ))
        //                                || shh.Match(m => m.OnField(of => of.Tags).Query(c.Query))
        //                                || shh.Nested(ne => ne
        //                                    .Path(pa => pa.Roles)
        //                                    .Query(qr => qr
        //                                    .Bool(b => b
        //                                        .Should(sh => sh
        //                                            .MatchPhrasePrefix(m => m.OnField(of => of.Roles.FirstOrDefault().Name.Suffix("raw")).Query(c.Query))
        //                                            || sh.MatchPhrasePrefix(m => m.OnField(of => of.Roles.FirstOrDefault().Description.Suffix("raw")).Query(c.Query))
        //                                            ))
        //                                    ))
        //                                ))
        //                            )
        //                        );
        //            if (matchResults.Documents.Count() > 0)
        //            {
        //                return matchResults;
        //            }
        //            else
        //            {
        //                return Connect().Search<ElasticResource>(s => s.Index(DefaultName)
        //                    .Skip((c.Page - 1) * ResPerPage)
        //                    .Take(ResPerPage)
        //                    .MinScore(MinScore)
        //                    .Query(q => q.FuzzyLikeThis(flt => flt.LikeText(c.Query))));
        //            }
        //        }
        //    }
        //}
        //public IQueryContainer QueryToggle(SearchContainer c, string employeeNumber)
        //{
        //    if (c.IsPersonal)
        //    {
        //        List<string> UNIDs = new List<string>();
        //        using (var _catalogDb = new CatalogDb())
        //        {
        //            Person per = _catalogDb.Persons.Where(a => a.EmployeeNumber == employeeNumber).FirstOrDefault();
        //            //TODO - сделать отдельный сервис
        //            UNIDs = _catalogDb.Resources
        //                //.Where(a => per.VIP == a.VIP || a.VIP == null)
        //                .Where(a => per.InnerEmployee == a.InnerEmployee || a.InnerEmployee == null)    //TODO - сделать условия по if(query.){query=query.where().....}
        //                .Where(a => !a.Paths.Any() || a.Paths.Any(pt => pt.Name != null && per.Path.Contains(pt.Name))) //есть ли вариант, что пути не будет у услуги вообще? у персоны?
        //                .Where(a => !a.Filials.Any() || a.Filials.Any(pt => pt.Code != null && pt.Code.Equals(per.Filial))).Select(s => s.UNID).ToList(); //Есть ли вариант, что филиала не будет вообще у услуги? у персоны?
        //            //TODO - сделать

        //        }
        //        return Query<ElasticResource>.Filtered(fltd => fltd
        //            .Query(q => q
        //                .Bool(b1 => b1
        //                    .Should(s1 => s1
        //                        .Nested(nt1 => nt1
        //                            .Path(pth1 => pth1.Resource)
        //                            .Query(qr1 => qr1
        //                                .Bool(bbb => bbb
        //                                    .Should(sss => sss
        //                                        .MatchPhrasePrefix(m => m.OnField(of => of.Resource.Code.Suffix("raw")).Query(c.Code))
        //                                        || sss.Match(m => m.OnField(of => of.Resource.Name).Query(c.Name))
        //                                        || sss.MatchPhrasePrefix(p2 => p2.OnField(o2 => o2.Resource.Owner.Suffix("raw")).Query(c.Owner))
        //                                        ))))))
        //                || q.Bool(b4 => b4.Should(s4 => s4.FuzzyLikeThis(f4 => f4.OnFields(o4 => o4.Tags).LikeText(c.Tags))))
        //                )
        //            .Filter(fltr => fltr.Nested(nes => nes.Path(pp => pp.Resource).Filter(d => d.Terms(tr => tr.Resource.UNID, UNIDs))))
        //            );
        //    }
        //    else
        //    {
        //        return Query<ElasticResource>.Bool(b1 => b1
        //                                        .Should(s1 => s1
        //                                            .Nested(nt1 => nt1
        //                                                .Path(pth1 => pth1.Resource)
        //                                                .Query(qr1 => qr1
        //                                                    .Bool(bbb => bbb
        //                                                        .Should(sss => sss
        //                                                            .MatchPhrasePrefix(m => m.OnField(of => of.Resource.Code.Suffix("raw")).Query(c.Code))
        //                                                            //|| sss.MatchPhrasePrefix(p2 => p2.OnField(o2 => o2.Resource.Name.Suffix("raw")).Query(c.Name))
        //                                                            || sss.Match(m => m.OnField(of => of.Resource.Name).Query(c.Name))
        //                                                            || sss.MatchPhrasePrefix(p2 => p2.OnField(o2 => o2.Resource.Owner.Suffix("raw")).Query(c.Owner))
        //                                                            ))))))
        //            || Query<ElasticResource>.Bool(b4 => b4.Should(s4 => s4.FuzzyLikeThis(f4 => f4.OnFields(o4 => o4.Tags).LikeText(c.Tags))));
        //    }
        //}
    }
}
