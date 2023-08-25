using Arabamcom2.Controllers;
using Arabamcom2.DbContext;
using Arabamcom2.DTOs;
using Arabamcom2.FluentValidation;
using Arabamcom2.IService;
using Arabamcom2.Models;
using Dapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Arabamcom2.Service
{
    public class AdvertService : IAdvertService
    {
        private readonly ConnectionHelper _context;
        private readonly IHttpContextAccessor _actionContextAccessor;
        private readonly ILogger<HomeController> _logger;
        public AdvertService(ConnectionHelper context, IHttpContextAccessor actionContextAccessor, ILogger<HomeController> logger)
        {
            _context = context;
            _actionContextAccessor = actionContextAccessor;
            _logger = logger;   
        }
        public async Task<Result<AdvertDto>> GetAdvertById(IdDto id)
        {
           try
            {

                //throw new Exception("Bu bir hata örneğidir.");
                string query = "SELECT * FROM Adverts FULL JOIN Cars ON Adverts.CarId = Cars.Id WHERE Adverts.Id = @Id";

                using (var connection = _context.CreateConnection())
                {

                    var companies = await connection.QueryFirstOrDefaultAsync<AdvertDto>(query, new { Id = id.Id });
                    if (companies != null)
                    {
                        return new Result<AdvertDto> { StatusCode = 200, Data = companies };
                    }
                    else
                    {
                        return new Result<AdvertDto> { StatusCode = 404, Data = companies };
                    }
                }

            }
           catch (Exception)
            {

                _logger.LogError(new Exception(), "Booom, there is an exception");

                Console.WriteLine($"An error occurred:");
                throw;
            }

        }

        public async Task<ResultList<AdvertCarDto>> GetAllAdvert(int pageSize, int pageNumber)
        {
            try
            {
               // throw new Exception("Bu bir hata örneğidir.");
                string query = "SELECT * FROM Adverts FULL JOIN Cars ON Adverts.CarId = Cars.Id";

                using (var connection = _context.CreateConnection())
                {
                    var companies = await connection.QueryAsync<AdvertCarDto>(query);

                    var Ad = new List<AdvertCarDto>();
                    return new ResultList<AdvertCarDto> { StatusCode = 200, Data = companies.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList() };
                }

            }
            catch (Exception)
            {
                _logger.LogError(new Exception(), "Booom, there is an exception");

                Console.WriteLine($"An error occurred:");
                throw;
            }

        }
        public async Task<Result<AdvertDto>> InsertAdvert(AdvertDto dto)
        {
            try
            {
                string query = "INSERT INTO Adverts(Id,title,createdAt,createdBy,active,carId,city) VALUES (@Id, @Title, @CreatedAt, @CreatedBy, @Active, @CarId, @City)";

                using (var connection = _context.CreateConnection())
                {
                    var advert = new AdvertDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = dto.Title,
                        CreatedAt = dto.CreatedAt,
                        CreatedBy = dto.CreatedBy,
                        Active = dto.Active,
                        City = dto.City,
                        CarId = dto.CarId
                    };
                    var result = await connection.ExecuteAsync(query, advert);
                    if (result > 0)
                    {
                        return new Result<AdvertDto> { StatusCode = 200, Data = advert };

                    }
                    else
                    {
                        throw new Exception("Failed to insert the animal record.");
                    }
                }
            }
            catch (Exception)
            {
                _logger.LogError(new Exception(), "Booom, there is an exception");

                Console.WriteLine($"An error occurred:");
                throw;
            }

        }

        public async Task<bool> DeleteAdvert(IdDto id)
        {
            try
            {
                string query = "DELETE FROM Adverts WHERE Id = @Id;";

                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.ExecuteAsync(query, new { Id = id.Id });
                    return result > 0;
                }
            }
            catch (Exception)
            {
                _logger.LogError(new Exception(), "Booom, there is an exception");

                Console.WriteLine($"An error occurred:");
                throw;
            }

        }

        public async Task<Result<AdvertDto>> UpdateAdvert(AdvertDto dto)
        {
            try
            {
                string query = @"
            UPDATE Adverts
            SET
                title = @Title,
                createdAt = @CreatedAt,
                createdBy = @CreatedBy,
                active = @Active,
                carId = @CarId,
                city = @City
            WHERE
                Id = @Id;";

                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.ExecuteAsync(query, dto);

                    if (result > 0)
                    {
                        return new Result<AdvertDto> { StatusCode = 200, Data = dto };
                    }
                    else
                    {
                        throw new Exception("Failed to update the advert record.");
                    }
                }
            }
            catch (Exception)
            {
                _logger.LogError(new Exception(), "Booom, there is an exception");

                Console.WriteLine("An error occurred while updating the advert.");
                throw;
            }
        }


        public async Task<Result<LogDto>> CreateLogEntry(string controller, string action, string message)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    string query = @"INSERT INTO Logs (Id,controller, action, message, createdDate) 
                                    VALUES (@Id,@Controller, @Action, @Message, @CreatedDate)";

                    var log = new LogDto
                    {
                        Id = Guid.NewGuid().ToString(),
                        Controller = controller,
                        Action = action,
                        Message = message,
                        CreatedDate = DateTime.Now
                    };
                    var result = await connection.ExecuteAsync(query, log);
                    if (result > 0)
                    {
                        return new Result<LogDto> { StatusCode = 200, Data = log };

                    }
                    else
                    {
                        throw new Exception("Failed to insert the advert record.");
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }

        }

        private void Log(Exception ex)
        {
            string controller = _actionContextAccessor.HttpContext.Request.RouteValues["controller"].ToString();
            string action = _actionContextAccessor.HttpContext.Request.RouteValues["action"].ToString();

            string message = ex.Message;

            CreateLogEntry(controller, action, message);
        }


    }
}
