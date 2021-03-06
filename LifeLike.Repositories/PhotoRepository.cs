﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LifeLike.Data.Models;
using LifeLike.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LifeLike.Repositories
{
    
    public  class PhotoRepository : IPhotoRepository
    {
        private readonly PortalContext _context;
        private readonly IEventLogRepository _logger;

        public PhotoRepository(PortalContext context, IEventLogRepository logger)
        {
            _logger = logger;
            _context = context;
        }

        public static readonly string PhotoPath =  "/photos/";


        public async Task<Result> Create(Photo model)
        {
            try
            {
                _context.Add(model);
                _context.SaveChanges();
                return  Result.Success;
                
            }
            catch (Exception e)
            {
              await _logger.AddException(e);
                return   Result.Failed;
            }    
        }

        public async Task<IEnumerable<Photo>> List()
        {
            return  await _context.Photos.ToListAsync();
        }

        public async Task<Photo> Get(long id)
        {
            try
            {
                return _context.Photos.FirstOrDefault(p => p.Id == id);                
            }
            catch (Exception e)
            {
                 await _logger.AddException(e);

                return   null;
            }
        }
  
        public async Task<Result> Update(Photo model)
        {
            try
            {
                _context.Update(model);
              await  _context.SaveChangesAsync();
                return  Result.Success;
                
            }
            catch (Exception e)
            {
                await _logger.AddException(e);

                return   Result.Failed;
            }        
        }

        public async Task<Result> Delete(Photo model)
        {
            try
            {
                _context.Remove(model);
                await _context.SaveChangesAsync();
                return  Result.Success;
                
            }
            catch (Exception e)
            {
               await _logger.AddException(e);

                return   Result.Failed;
            }
        }

        public async Task<Result> Create(Photo photo, long modelGalleryId)
        {
            try
            {
                var gallery = _context.Galleries
                    .Where(p => p.Id == modelGalleryId)
                    .Include(p=>p.Photos)
                    .SingleOrDefault();
                if (gallery == null) return Result.Failed;
                gallery.Photos.Add(photo);

                _context.SaveChanges();
                return  Result.Success;
            }
            catch (Exception e)
            {
               await _logger.AddException(e);

                return   Result.Failed;
            }           
        }
    }
    
    public interface IPhotoRepository : IRepository<Photo>
    {
        Task<Result> Create(Photo photo, long modelGalleryId);
    }
}