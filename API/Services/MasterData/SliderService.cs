using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models.MasterData;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Data.Entity.MasterData;
using API.Service.MasterData.Interfaces;

namespace API.Service.MasterData
{
    public class SliderService : ISliderService
    {
        private readonly ReactDbContext _context;


        public SliderService(ReactDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveSlider(Slider slider)
        {
            if (slider.Id != 0)
            {
                _context.Sliders.Update(slider);

                await _context.SaveChangesAsync();

                return true;
            }

            await _context.Sliders.AddAsync(slider);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Slider>> GetAllSlider()
        {
            return await _context.Sliders.ToListAsync();
        }

        public async Task<Slider> GetSliderById(int id)
        {
            return await _context.Sliders.FindAsync(id);
        }

        public async Task<bool> DeleteSliderById(int id)
        {
            _context.Sliders.Remove(_context.Sliders.Find(id));

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Slider>> GetAllActiveSlider()
        {
            return await _context.Sliders.Where(x=>x.isActive == true).ToListAsync();
        }

        public async Task<bool> IsActive(int id)
        {
           Slider slider = await  _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);

           if (slider.isActive == true)
           {
               slider.isActive = false;
           }
           else
           {
               slider.isActive = true;
           }

           _context.Sliders.Update(slider);
           await _context.SaveChangesAsync();


           return true;
        }

    }
}
