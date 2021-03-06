using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Restaurant.BLL.DTOobjects;
using Restaurant.BLL.Interfaces;
using Restaurant.BLL.Profiles;
using Restaurant.DAL.Entities;
using Restaurant.DAL.Interfaces;

namespace Restaurant.BLL.Services
{
    public class IngredientService : IIngredientService
    {
        private IUnitOfWork _db;
        private Mapper _mapper;
        
        public void Dispose()
        {
            _db.Dispose();
        }
        
        public IngredientService(IUnitOfWork db)
        {
            _db = db;
            
            _mapper = new Mapper(
                new MapperConfiguration(
                    cfg=>cfg
                        .AddProfile<IngredientProfile>()));
        }

        public IEnumerable<IngredientDTO> GetAll()
        {
            var ingredients = _mapper
                .Map<IEnumerable<IngredientDTO>>(
                    _db.Ingredients.GetAll());
            
            return ingredients;
        }
        
        public void Add(string ingredient)
        {
            Ingredient ingred = new Ingredient();
            
            if (!(_db.Ingredients.GetAll().Any(elem=>elem.Name == ingredient)))
            {
                ingred.Name = ingredient;
                _db.Ingredients.Create(_mapper.Map<Ingredient>(ingred));
                _db.Save();
            }
        }

        public void Update(int oldIngredient, string newIngredient)
        {
            Ingredient ingr = _db.Ingredients.GetAll()
                .Single(elem => elem.Id == oldIngredient);

            ingr.Name = newIngredient;
            
            _db.Ingredients.Update(_mapper.Map<Ingredient>(ingr));
            _db.Save();
        }
        
        public void Delete(string ingredient)
        {
            Ingredient ingr = _db.Ingredients.GetAll()
                .Single(elem => elem.Name == ingredient);
            
            if (!ingr.Equals(null) && ingr.Dishes.Count == 0)
            {
                _db.Ingredients.Delete(ingr.Id);
                _db.Save();
            }
            else
            {
                throw new Exception("Sorry, this operation can't be done. Try again!");
            }
        }
    }
}