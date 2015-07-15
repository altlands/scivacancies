using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using NPoco;
using SciVacancies.ReadModel.Pager;

namespace SciVacancies.WebApp
{
    public static class MappingExtensions
    {
        /*create mapp*/

        public static IMappingExpression<TSrc, TDest> IncludePagedResultMapping<TSrc, TDest>(this IMappingExpression<TSrc, TDest> expression)
        {
            Mapper.CreateMap<Page<TSrc>, PagedList<TDest>>()
                .ForMember(dest => dest.PageSize, m => m.MapFrom(src => src.ItemsPerPage))
                .ForMember(dest => dest.CurrentPage, m => m.MapFrom(src => src.CurrentPage))
                .ForMember(dest => dest.TotalItems, m => m.MapFrom(src => src.TotalItems))
                .ForMember(dest => dest.TotalPages, m => m.MapFrom(src => src.TotalPages))
                .ForMember(dest => dest.FirstRowIndexOnPage, m => m.MapFrom(src => src.Items != null && src.Items.Count > 0 ?  (src.ItemsPerPage * (src.CurrentPage - 1)) + 1 : 0))
                .ForMember(dest => dest.LastRowIndexOnPage, m => m.MapFrom(src => src.Items != null && src.Items.Count > 0 ? (src.ItemsPerPage * (src.CurrentPage - 1)) + src.Items.Count : 0 ))
                .ForMember(dest => dest.Items, m => m.MapFrom(src => Mapper.Map<List<TDest>>(src.Items)))
                ;
            return expression;
        }


        /*use map*/

        public static PagedList<TSrc> MapToPagedList<TSrc>(this Page<TSrc> source)
        {
            return Mapper.Map<PagedList<TSrc>>(source) ;
        }

        public static PagedList<TDst> MapToPagedList<TSrc, TDst>(this Page<TSrc> source)
        {
            return Mapper.Map<PagedList<TDst>>(source);
        }
    }
}
