using ApplicationCore.Models;
using Infrastructure.EF.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using WebAPI.Controllers;

namespace WebAPI.Mappers
{
    public static class QuizMappers
    {
        public static QuizItem FromEntityToQuizItem(QuizItemEntity entity)
        {
            return new QuizItem(
                entity.Id,
                entity.Question,
                entity.IncorrectAnswers.Select(e => e.Answer).ToList(),
                entity.CorrectAnswer);
        }

        public static Quiz FromEntityToQuiz(QuizEntity entity)
        {
            var items = entity.Items.Select(FromEntityToQuizItem).ToList();
            return new Quiz(
                entity.Id,
                items,
                entity.Title
                );
        }
    }
}