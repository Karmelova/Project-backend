using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.EF.Entities;
using Microsoft.EntityFrameworkCore;
using WebAPI.Mappers;

namespace WebAPI.Services
{
    public class QuizUserServiceEF : IQuizUserService
    {
        private readonly ApplicationDbContext _context;

        public QuizUserServiceEF(ApplicationDbContext context)
        {
            _context = context;
        }

        //public IEnumerable<Quiz> FindAllQuizzes()
        //{
        //    return _context
        //        .Quizzes
        //        .AsNoTracking()
        //        .Include(q => q.Items)
        //        .ThenInclude(i => i.IncorrectAnswers)
        //        .Select(QuizMappers.FromEntityToQuiz)
        //        .ToList();
        //}

        //public Quiz? FindQuizById(int id)
        //{
        //    var entity = _context
        //        .Quizzes
        //        .AsNoTracking()
        //        .Include(q => q.Items)
        //        .ThenInclude(i => i.IncorrectAnswers)
        //        .FirstOrDefault(e => e.Id == id);
        //    return entity is null ? null : QuizMappers.FromEntityToQuiz(entity);
        //}

        public Quiz CreateAndGetQuizRandom(int count)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Quiz> FindAllQuizzes()
        {
            throw new NotImplementedException();
        }

        public Quiz? FindQuizById(int id)
        {
            throw new NotImplementedException();
        }

        public List<QuizItemUserAnswer> GetUserAnswersForQuiz(int quizId, int userId)
        {
            throw new NotImplementedException();
        }

        public QuizItemUserAnswer SaveUserAnswerForQuiz(int quizId, int quizItemId, int userId, string answer)
        {
            var quizzEntity = _context.Quizzes.FirstOrDefault(q => q.Id == quizId);
            if (quizzEntity is null)
            {
                throw new QuizNotFoundException($"Quiz with id {quizId} not found");
            }
            var item = _context.QuizItems.FirstOrDefault(qi => qi.Id == quizItemId);
            if (item is null)
            {
                throw new QuizItemNotFoundException($"Quiz item with id {quizId} not found");
            }
            QuizItemUserAnswerEntity entity = new QuizItemUserAnswerEntity()
            {
                QuizId = quizId,
                UserAnswer = answer,
                UserId = userId,
                QuizItemId = quizItemId
            };
            var savedEntity = _context.Add(entity).Entity;
            _context.SaveChanges();
            return new QuizItemUserAnswer()
            {
                QuizId = savedEntity.QuizId,
                UserId = savedEntity.UserId,
                Answer = savedEntity.UserAnswer
            };
        }
    }
}