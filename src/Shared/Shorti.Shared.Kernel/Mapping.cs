using AutoMapper;

namespace Shorti.Shared.Kernel
{
    public static class Mapping
    {
        public static TTarget Map<TSource, TTarget>(TSource source)
        {
            var conf = new MapperConfiguration(c =>
                c.CreateMap<TSource, TTarget>());

            Mapper mapper = new(conf);

            var result = mapper.Map<TSource, TTarget>(source);

            return result;
        }
    }
}
