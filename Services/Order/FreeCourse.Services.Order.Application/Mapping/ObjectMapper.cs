using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Mapping
{
    public static class ObjectMapper
    {
        // Lazy kullanarak bu mapping helper sadece kullanıldığı zaman initialize edilmesini sağlıyoruz.
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CustomMapping>();
            });

            return config.CreateMapper();
        });

        //Sadece bu method çalıştığı zaman yukarıdaki işlemler gerçekleştirilecek
        public static IMapper Mapper => lazy.Value;
    }
}
