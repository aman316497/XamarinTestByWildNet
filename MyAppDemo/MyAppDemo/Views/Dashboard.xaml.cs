using MyAppDemo.Models;
using MyAppDemo.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyAppDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        List<QuestionModel> deleteList { get; set; }
        List<QuestionModel> completeList { get; set; }

        List<QuestionModel> questList { get; set; }
        private DashboardViewModel viewModel;
        public Dashboard()
        {
            InitializeComponent();
            OnLoad();
        }
        async private void OnLoad()
        {
            try
            {
                RestServices restServices= new RestServices();
                var data = await restServices.GetQuestionsAsync();
                if (data != null)
                {
                    if (Application.Current.Properties.ContainsKey("SkippedQuestSrNo"))
                    {
                        var deleteSrNo = Application.Current.Properties["SkippedQuestSrNo"].ToString();
                        if (!string.IsNullOrEmpty(deleteSrNo))
                        {
                            deleteList = new List<QuestionModel>();
                            foreach(var srNo in deleteSrNo.Split(','))
                            {
                                var model = questList.FirstOrDefault(x => x.srNo == Convert.ToInt32(srNo));
                                deleteList.Add( model) ;
                                questList.Remove(model);
                            }
                        }
                    }
                        questList = data;
                    viewModel = new DashboardViewModel(questList.First());
                    BindingContext = viewModel;
                }

            }catch(Exception ex) {
            }
        }
        async private void Button_Clicked(object sender, EventArgs e) 
        {
            try
            {
                var btn = (Button)sender;
                switch(btn.AutomationId)
                {
                    case "btnNext":
                        SkipOrCompleteModel("done", viewModel.question);
                        break;
                    case "btnSkip":
                        SkipOrCompleteModel("skip", viewModel.question);
                        break;
                }
            }catch(Exception ex) { Console.WriteLine(ex.ToString()); }  
        }
        async private void SkipOrCompleteModel(string action, QuestionModel ques)
        {
            switch(action)
            {
                case "skip":
                    if(deleteList == null) deleteList= new List<QuestionModel>();
                    deleteList.Add(ques);
                    questList.Remove(ques);
                    break;

                case "done":
                    if(completeList == null) completeList= new List<QuestionModel>();
                    completeList.Add(ques);
                    questList.Remove(ques);
                    break;
            }
            if (questList.Count == 0)
            {
                if (Application.Current.Properties.ContainsKey("SkippedQuestSrNo"))
                    Application.Current.Properties.Remove("SkippedQuestSrNo");  
                
                if (deleteList != null)
                {
                    var deletedSrNo = string.Join(",", deleteList.Select(x => x.srNo).ToArray());
                    Application.Current.Properties.Add("SkippedQuestSrNo", deletedSrNo);
                    Application.Current.SavePropertiesAsync();
                }
                await Navigation.PopAsync();
            }
            else
            {
                viewModel = new DashboardViewModel(questList.First());
                BindingContext = viewModel;
            }
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                var tap= (Frame) sender;
                switch(tap.AutomationId)
                {
                    case "tpFrame1":
                        frSelecton1.Background = null;
                        frSelecton1.BackgroundColor = Color.FromHex("D540FF");
                        frSelecton2.Background = frSelecton3.Background = frSelecton4.Background = new LinearGradientBrush(gradientStops: new GradientStopCollection() {  
                        new GradientStop(color : Color.FromHex("9DCEFF"), 0f) , new GradientStop(color : Color.FromHex("92A3FD"), 1f)});
                        break;
                    case "tpFrame2":
                        frSelecton2.Background = null ;
                        frSelecton2.BackgroundColor = Color.FromHex("D540FF"); 
                        frSelecton1.Background = frSelecton3.Background = frSelecton4.Background = new LinearGradientBrush(gradientStops: new GradientStopCollection() {
                        new GradientStop(color : Color.FromHex("9DCEFF"), 0f) , new GradientStop(color : Color.FromHex("92A3FD"), 1f)});
                        break;

                    case "tpFrame3":
                        frSelecton3.Background = null ;
                        frSelecton3.BackgroundColor = Color.FromHex("D540FF"); 
                        frSelecton2.Background = frSelecton1.Background = frSelecton4.Background = new LinearGradientBrush(gradientStops: new GradientStopCollection() {
                        new GradientStop(color: Color.FromHex("9DCEFF"),0f), new GradientStop(color: Color.FromHex("92A3FD"),1f)});
                        break;
                    case "tpFrame4":

                        frSelecton4.Background = null;
                        frSelecton4.BackgroundColor = Color.FromHex("D540FF"); 
                        frSelecton2.Background = frSelecton3.Background = frSelecton1.Background = new LinearGradientBrush(gradientStops: new GradientStopCollection() {
                        new GradientStop(color: Color.FromHex("9DCEFF"),0f), new GradientStop(color: Color.FromHex("92A3FD"),1f)});
                        break;

                }
                btnNext.IsEnabled = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    
        
    }
    internal class DashboardViewModel : INotifyPropertyChanged
    { 
        private QuestionModel _question { get; set; }
        public QuestionModel question { get => _question; set
            {
                _question= value;
            } }
        public string QuestonTitle { get => question.text ?? string.Empty; }
        public string Option1 { get => question.questionAnswers[0].username ?? string.Empty; }
        public string Option2 { get=> question.questionAnswers[1].username ?? string.Empty;  }
        public string Option3 { get=>question.questionAnswers[2].username ?? string.Empty;  }
        public string Option4 { get=>question.questionAnswers[3].username ?? string.Empty;  }


        public DashboardViewModel(QuestionModel questionModel ) {
            _question = questionModel;

        }

        private void OnPropertyChanged(string invokeName = null)
        {
            try
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(invokeName));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}