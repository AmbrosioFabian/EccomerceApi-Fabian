﻿using AplicationLayer;
using Data;
using EnterpriseLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq.Expressions;

namespace Repository
{
    public class CartRepository : IRepository<Cart>, IRepositorySearch<CartModel, Cart>
    {

        private readonly AppDbContext _dbContext;

        public CartRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Cart cart)
        {
            var cartModel = new CartModel();
            cartModel.UserId = cart.UserId;
            cartModel.CreatedAt = cart.CreatedAt;
            cartModel.CartItems = cart.CartItems.Select(ci => new CartItemModel
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                CreatedAt = ci.CreatedAt

            }).ToList();

            await _dbContext.Carts.AddAsync(cartModel);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Cart>> GetAllAsync() 
            => await _dbContext.Carts
                .Select(c => new Cart(c.Id, c.UserId, c.CreatedAt, 
                                        _dbContext.CartItems
                                            .Where(ci => ci.CartId == c.Id)
                                            .Select(ci => new CartItem(ci.ProductId, ci.Quantity, ci.CreatedAt))
                                            .ToList()
                                    )
                ).ToListAsync();

        public async Task<Cart> GetByIdAsync(int id)
        {
            var cartModel = await _dbContext.Carts.FindAsync(id);
            return new Cart(cartModel.Id, cartModel.UserId, cartModel.CreatedAt,
                                _dbContext.CartItems
                                    .Where(ci => ci.CartId == cartModel.Id)
                                    .Select(ci => new CartItem(ci.ProductId, ci.Quantity, ci.CreatedAt))
                                    .ToList()
                            );
        }

        public async Task<IEnumerable<Cart>> GetAsync(Expression<Func<CartModel, bool>> predicate)
        {
            var cartsModel = await _dbContext.Carts.Include("CartItems").Where(predicate).ToListAsync();
            var carts = new List<Cart>();

            foreach (var cartModel in cartsModel)
            {
                var cartItems = new List<CartItem>();
                foreach (var cartItemModel in cartModel.CartItems)
                {
                    cartItems.Add(new CartItem(cartItemModel.ProductId, cartItemModel.Quantity, cartItemModel.CreatedAt));
                }
                carts.Add(new Cart(cartModel.Id, cartModel.UserId, cartModel.CreatedAt, cartItems));
            }
            return carts;
        }

        public async Task UpdateAsync(int id, Cart entity)
        {
            var cartModel = await _dbContext.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.Id == id);

            if (cartModel == null)
            {
                throw new Exception($"Cart with ID {id} not found.");
            }

            cartModel.UserId = entity.UserId;
            cartModel.CreatedAt = entity.CreatedAt;

            _dbContext.CartItems.RemoveRange(cartModel.CartItems);

            // Luego, agregar los nuevos items
            cartModel.CartItems = entity.CartItems.Select(ci => new CartItemModel
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                CreatedAt = ci.CreatedAt,
                CartId = cartModel.Id 
            }).ToList();

            await _dbContext.SaveChangesAsync();
        }


        public async Task DeleteAsync(int id)
        {
            // Buscar el carrito por su ID
            var cart = await _dbContext.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.Id == id);

            if (cart == null)
            {
                throw new Exception($"Cart with ID {id} not found.");
            }

            _dbContext.Carts.Remove(cart);
            await _dbContext.SaveChangesAsync();
        }
    }
}
