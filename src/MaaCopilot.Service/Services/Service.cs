using AutoMapper;
using MaaCopilot.DataTransferObjects;
using MaaCopilot.Interfaces.DataAccess;
using MaaCopilot.Interfaces.ORM;
using MaaCopilot.Interfaces.Service;
using MaaCopilot.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaaCopilot.Service.Services
{
    public abstract class Service<U, T> : IService<U, T> where U : class where T : class
    {
        protected IDBHelper _iDBHelper;
        protected IUnitOfWork _unitOfWork;
        public Service(IDBHelper dBHelper, IUnitOfWork unitOfWork)
        {
            _iDBHelper = dBHelper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IServiceResponse<U>> AddAsync(U dto)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T, U>().ReverseMap();
            });
            IMapper mapper = config.CreateMapper();
            try
            {
                if (dto == null)
                    return new ServiceResponse<U>(false, "Invalid Request");
                var data = mapper.Map<U, T>(dto);
                var id = await _unitOfWork.Repository<T>().AddAsync(data, new List<string>().ToArray());
                return new ServiceResponse<U>(id > 0, id > 0 ? "Success" : "Failed", dto);
            }
            catch (Exception ex)
            {
                return new ServiceResponse<U>(false, ex.Message, null);
            }
        }

        public Task<IServiceResponse<U>> Search(SearchDTO request)
        {
            throw new NotImplementedException();
        }

        public Task<IServiceResponse<U>> UpdateAsync(U data)
        {
            throw new NotImplementedException();
        }
    }
}
