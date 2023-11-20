using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoppingCartApi.Models;
using ShoppingCartApi.Models.Dto;
using ShoppingCartApi.Repository;

namespace ShoppingCartApi.Services.Cards
{
    public class CardCommandService : ICardCommandService
    {
        private readonly IRepository<CardHeader> _cartHeaderRepository;
        private readonly IRepository<CardDetails> _cartDetailsRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public CardCommandService(
            IRepository<CardHeader> cartHeaderRepository,
            IRepository<CardDetails> cartDetailsRepository,
            IRepository<Product> productRepository,
            IMapper mapper)
        {
            _cartHeaderRepository = cartHeaderRepository;
            _cartDetailsRepository = cartDetailsRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<bool> ClearCard(string userId)
        {
            var cartHeaderFromDb = await _cartHeaderRepository.FirstOrDefaultAsync(u => u.UserId == userId);
            if (cartHeaderFromDb != null)
            {
                var cardDetails = await _cartDetailsRepository.Query(e => e.Id == cartHeaderFromDb.Id).ToListAsync();
                await _cartDetailsRepository.RemoveRangeAndSaveAsync(cardDetails);
                return true;

            }
            return false;
        }

        public async Task<CardDto> CreateUpdateCard(CardDto cardDto)
        {
            Card card = _mapper.Map<Card>(cardDto);

            var product = await _productRepository.FirstOrDefaultAsync(e => e.Id == card.CardDetails.FirstOrDefault().Product.Id);

            if (product is null)
            {
                await _productRepository.AddAndSaveAsync(card.CardDetails.FirstOrDefault().Product);
            }

            //check if header is null
            var cardHeader = await _cartHeaderRepository.Query(e => e.UserId == card.CardHeader.UserId).AsNoTracking().FirstOrDefaultAsync();

            if (cardHeader == null)
            {
                //create header and details
                await _cartHeaderRepository.AddAndSaveAsync(card.CardHeader);

                var cardDetails = card.CardDetails.FirstOrDefault();
                cardDetails.CardHeader = card.CardHeader;
                cardDetails.Product = product;
                await _cartDetailsRepository.AddAndSaveAsync(cardDetails);
            }
            else
            {
                //if header is not null
                //check if details has same product
                var cardDeatils = await _cartDetailsRepository
                    .Query(e => e.CardHeader.Id == cardHeader.Id && e.Product.Id == card.CardDetails.FirstOrDefault().Product.Id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (cardDeatils == null)
                {
                    //create details
                    var cardDeatilsToAdd = card.CardDetails.FirstOrDefault();
                    cardDeatilsToAdd.CardHeader= cardDeatils.CardHeader;
                    cardDeatilsToAdd.Product = product;
                    await _cartDetailsRepository.AddAndSaveAsync(cardDeatilsToAdd);
                }
                else
                {
                    //update the count / cart details
                    var cardDeatilsToUpdate = card.CardDetails.FirstOrDefault();
                    cardDeatilsToUpdate.Product = product;
                    cardDeatilsToUpdate.Count += cardDeatils.Count;
                    cardDeatilsToUpdate.Id = cardDeatils.Id;
                    cardDeatilsToUpdate.CardHeader = cardDeatils.CardHeader;
                    await _cartDetailsRepository.ModifyAndSaveAsync(cardDeatilsToUpdate);
                }
            }
            return _mapper.Map<CardDto>(card);
        }


        public async Task<bool> RemoveFromCard(long cardDetailsId)
        {
            try
            {
                var cardDetails = await _cartDetailsRepository.FindAsync(cardDetailsId);

                int totalCountOfCardItems = await _cartDetailsRepository.CountAsync(e => e.CardHeader.Id == cardDetails.CardHeader.Id);

                await _cartDetailsRepository.RemoveAndSaveAsync(cardDetails);  
                
                if (totalCountOfCardItems == 1)
                {
                    var cartHeaderToRemove = await _cartHeaderRepository.FindAsync(cardDetails.CardHeader.Id);

                    await _cartHeaderRepository.RemoveAndSaveAsync(cartHeaderToRemove);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
