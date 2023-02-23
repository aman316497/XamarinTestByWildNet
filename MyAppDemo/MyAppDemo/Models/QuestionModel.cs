using System;
using System.Collections.Generic;
using System.Text;

namespace MyAppDemo.Models
{
    public class ResponseModel
    {
        public List<QuestionModel> questions { get; set; }
    }
    public class QuestionModel
    {
        public int srNo { get; set; }
        public string text { get; set; }
        public List<QuestionAnswerModel> questionAnswers { get; set; }
    }
    public class QuestionAnswerModel
    {
        public string userId { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string username { get => string.Join(" ", firstname, lastname); }

    }
}
