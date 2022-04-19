using BookStore.Models;
using BookStore.Models.Data;
using BookStore.Models.ViewModels;
using BookStore.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookStore.Controllers
{
    [Route("api/Category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        /// <summary>
        /// Returns list of existing categories.
        /// </summary>
        /// <param name="pageIndex">pageIndex as int. Starts from 0</param>
        /// <param name="pageSize">pageSize as int.</param>
        /// <returns>List of categories.</returns>
        [HttpGet]
        [Route("Get")]
        public IActionResult GetCategories(int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                CategoryRepository repo = new CategoryRepository();
                BaseList<Category> categories = repo.GetAll(pageIndex, pageSize);
                ResultStateWithModel<BaseList<Category>> resultState = new ResultStateWithModel<BaseList<Category>>();
                resultState.Data = categories;
                return StatusCode((int)HttpStatusCode.OK, resultState);
            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }


        /// <summary>
        /// Add new categories.
        /// </summary>
        /// <param name="category">category</param>
        /// <returns>Added category.</returns>
        [HttpPut]
        [Route("Add")]
        public IActionResult AddCategory(Category category)
        {
            try
            {

                CategoryRepository categoryRepository = new CategoryRepository();
                Category result = categoryRepository.Add(category);

                ResultStateWithModel<Category> addCategoryResultState = new ResultStateWithModel<Category>();
                addCategoryResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, addCategoryResultState);

            }
            catch(Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);

            }
        }

        /// <summary>
        /// Update existing category.
        /// </summary>
        /// <param name="category">category</param>
        /// <returns>updated category.</returns>
        [HttpPost]
        [Route("Update")]
        public IActionResult UpdateCategory(Category category)
        {
            try
            {
                CategoryRepository categoryRepository = new CategoryRepository();

                Category result = categoryRepository.Update(category);

                ResultStateWithModel<Category> updateCategoryResultState = new ResultStateWithModel<Category>();
                updateCategoryResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, updateCategoryResultState);
            }
            catch(Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }

        /// <summary>
        /// Delete existing category.
        /// </summary>
        /// <param name="category">category</param>
        /// <returns>deleted category.</returns>
        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                CategoryRepository categoryRepository = new CategoryRepository();

                Category result = categoryRepository.Delete(id);

                ResultStateWithModel<Category> deleteCategoryResultState = new ResultStateWithModel<Category>();
                deleteCategoryResultState.Data = result;

                return StatusCode((int)HttpStatusCode.OK, deleteCategoryResultState);
            }
            catch (Exception ex)
            {
                ResultState resultState = new ResultState(Messages.GeneralExceptionCode, "Failed", ex.GetBaseException().Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, resultState);
            }
        }


    }
}
