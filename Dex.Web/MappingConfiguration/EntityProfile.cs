using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dex.Common.DTO;
using Dex.DataAccess.Models;

namespace Dex.Infrastructure.Mapping
{
    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            LogMap();
        }

        private void LogMap()
        {
            CreateMap<Log, LogDTO>()
                .ReverseMap();
        }
    }
}
