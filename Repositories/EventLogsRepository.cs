﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LifeLike.Models;
using LifeLike.Repositories;

namespace LifeLike.Repositories
{
    public class EventLogsRepository : IEventLogRepository
    {
        private readonly PortalContext _context;

        public EventLogsRepository(PortalContext context)
        {
            _context = context;
        }
        
        public async Task<Result> AddExceptionLog(Exception e)
        {
            try
            {
               return await Create(EventLog.Generate(e));          
            }
            catch (System.Exception)
            {
                return Result.Failed;
            }
        }

        public async Task<Result> AddStat(string id,string action, string controller)
        {
            try
            {
                return await Create(EventLog.Generate(id, action, controller));
            }
            catch (System.Exception)
            {
                return Result.Failed;
            }
        }
        public async Task<Result> Add(EventLog model)
        {
            try
            {             
               return  await Create(model);
            }
            catch (Exception)
            {
                return Result.Success;
            }        
        }
        public async Task<Result> Create(EventLog model)
        {
            try
            {             
                await _context.EventLogs.AddAsync(model);
                await _context.SaveChangesAsync();
                return Result.Success;
            }
            catch (Exception)
            {
            //    await AddExceptionLog(e);
                return Result.Success;
            }        
        }

        public async Task<IEnumerable<EventLog>> List()
        {
            return _context.EventLogs.ToList();
        }

        public async Task<IEnumerable<EventLog>> List(EventLogType type)
        {
            return _context.EventLogs.Where(p => p.Type == type).ToList();
        }

        public async Task<Result> LogInformation(int i, string information)
        {
            return Result.Failed;
        }

        public async Task<Result> LogInformation(EventLogType result, string message)
        {
            try
            {
                await Create(EventLog.Generate(result,message));
                return Result.Success;
            }
            catch (System.Exception e)
            {
                await AddExceptionLog(e);
                return Result.Success;
            }

        }

        public async Task<Result>  ClearLogs()
        {
            try
            {
                  _context.EventLogs.RemoveRange(_context.EventLogs.Where(p=>p.Type!=EventLogType.Statistic));            
           
                await _context.SaveChangesAsync();
                return Result.Success;
            }
            catch (System.Exception)
            {
                
                throw;
            }
          
             
        }

        public  async Task<EventLog> Get(long id)
        {
            return _context.EventLogs.FirstOrDefault(p=>p.Id==id);
        }

        public async Task<Result> Update(EventLog model)
        {
            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                return Result.Success;

            }
            catch (Exception e)
            {
                await AddExceptionLog(e);
                return Result.Failed;
            }
            
        }

        public async Task<Result> Delete(EventLog model)
        {
            try
            {
                _context.Remove(model);
                await _context.SaveChangesAsync();
                return Result.Success;

            }
            catch (Exception e)
            {
                await AddExceptionLog(e);
                return Result.Failed;
            }
        }
          public async Task<Result> DeleteAll()
        {
            try
            {
                _context.EventLogs.RemoveRange(_context.EventLogs.ToList());
                await _context.SaveChangesAsync();
                return Result.Success;

            }
            catch (Exception e)
            {
                await AddExceptionLog(e);
                return Result.Failed;
            }
        }
       
    }
    
    public interface IEventLogRepository : IRepository<EventLog>
    {
        Task<Result> AddExceptionLog(Exception e);
        Task<Result> AddStat(string id, string action, string controller);
        Task<IEnumerable<EventLog>> List(EventLogType type);
        Task<Result>  LogInformation(int i, string information);
        Task<Result>  LogInformation(EventLogType result, string message);
        Task<Result> ClearLogs();
        Task<Result> Add(EventLog eventLog);
        Task<Result> DeleteAll();
    }
}