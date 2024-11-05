using AutoMapper;
using FluffyPaw_Application.DTO.Request.TransactionRequest;
using FluffyPaw_Application.DTO.Response.TransactionResponse;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Domain.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWalletService _walletService;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper, IWalletService walletService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _walletService = walletService;
        }

        public async Task<bool> AddTransactions(TransactionRequest transactionRequest)
        {
            var transaction = _mapper.Map<Transaction>(transactionRequest);
            transaction.CreateTime = CoreHelper.SystemTimeNow;

            _unitOfWork.TransactionRepository.Insert(transaction);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> CheckReceipt(long orderCode)
        {
            return _unitOfWork.TransactionRepository.Get(oc => oc.OrderCode.Equals(orderCode)).Any();
        }

        public async Task<IEnumerable<TransactionResponse>> GetTransactions()
        {
            var wallet = await _walletService.ViewWallet();
            var list = _unitOfWork.TransactionRepository.Get(w => w.WalletId.Equals(wallet.Id));
            if (!list.Any()) throw new CustomException.DataNotFoundException("Không có lịch sử giao dịch.");

            return _mapper.Map<IEnumerable<TransactionResponse>>(list);
        }
    }
}
