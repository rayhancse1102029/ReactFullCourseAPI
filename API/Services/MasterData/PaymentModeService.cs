using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Data.Entity.MasterData;
using API.Services.MasterData.Interfaces;

namespace API.Services.MasterData
{
    public class PaymentModeService : IPaymentModeService
    {
        private readonly ReactDbContext _context;

        public PaymentModeService(ReactDbContext context)
        {
            _context = context;
        }

        #region Payment Mode
        public async Task<int> SavePaymentMode(PaymentMode paymentMode)
        {
            if (paymentMode.Id != 0)
                _context.PaymentModes.Update(paymentMode);
            else
                _context.PaymentModes.Add(paymentMode);
            await _context.SaveChangesAsync();
            return paymentMode.Id;
        }

        public async Task<IEnumerable<PaymentMode>> GetAllPaymentMode()
        {

            List<PaymentMode> paymentModes = await _context.PaymentModes.AsNoTracking().ToListAsync();

            return paymentModes;
        }

        public async Task<bool> DeletePaymentbyId(int id)
        {
            _context.PaymentModes.Remove(_context.PaymentModes.Find(id));
            return 1 == await _context.SaveChangesAsync();
        }

        #endregion

    }
}
