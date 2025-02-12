using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Application;
using Domain;
using EFDataAccess;

namespace Implementation.Logging
{
    public class EFDatabaseLogger : IUseCaseLogger
    {
        private readonly BlogContext _context;

        public EFDatabaseLogger(BlogContext context)
        {
            _context = context;
        }

        public void Log(IUseCase useCase, IApplicationActor actor, object data)
        {
            try
            {
                _context.UseCaseLogs.Add(new UseCaseLog
                {
                    Date = DateTime.UtcNow,
                    Actor = actor.Identity,
                    Data = JsonConvert.SerializeObject(data),
                    UseCaseName = useCase.Name.ToString()
                });

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}