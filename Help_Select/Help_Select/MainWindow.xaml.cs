using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;



namespace Help_Select
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    ///    

    public partial class MainWindow : Window
    {        
        int input_count = 1;

        List<string> input_value = new List<string> { };

        DependencyObject parentCT;

        public MainWindow()
        {
            InitializeComponent();
                        
            parentCT = this.input_tab.Parent;
            
        }

        //입력값 중에서 랜덤으로 선택 해 출력해주는 함수
        //Pick function
        private void Help_btn_Click(object sender, RoutedEventArgs e)
        {
            input_number.Visibility = Visibility.Collapsed;
                        

            //입력 값 리스트 생성

            if (input_count == 1)
            {
                MessageBox.Show("선택지를 추가해주세요", "Help Pick");
                return;
            }
            
            foreach (TextBox txtbox_ in parentCT.FindVisualChildren<TextBox>())
            {
                //값 맞는지 테스트
                //test.Text += txtbox_.Text;

                string str_ = txtbox_.Text;

                if (str_ == "?")
                {
                    //do nothing
                }
                else if (String.IsNullOrEmpty(str_) || str_ == " ")
                {
                    MessageBox.Show("빈칸을 채워주세요", "Help Pick");
                    return;
                }
                else
                {
                    input_value.Add(str_);
                }
                txtbox_.Focusable = false;
            }

            Random rnd = new Random();
            int result = rnd.Next(1, input_count);

            result_text.Text = input_value[result - 1];

            result_tab.Visibility = Visibility.Visible;
            help_btn.Visibility = Visibility.Hidden;
        }

        // All value, variables Reset
        private void Reset_btn_Click(object sender, RoutedEventArgs e)
        {
            inform_stack.Visibility = Visibility.Visible;
            select_tab.Visibility = Visibility.Visible;

            input_tab.Visibility = Visibility.Collapsed;


            input_btn.Visibility = Visibility.Visible;

            help_btn.Visibility = Visibility.Collapsed;
            reset_btn.Visibility = Visibility.Collapsed;

            result_tab.Visibility = Visibility.Collapsed;

            input_number.Visibility = Visibility.Visible;
                        
            //reset
            input_tab.Children.Clear();
            input_value.Clear();
            input_count = 1;

            foreach (TextBox txtbox_ in parentCT.FindVisualChildren<TextBox>())
            {
                txtbox_.Focusable = true;
            }
        }

        //input_count++
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (input_count == 10)
            {
                MessageBox.Show("더이상 추가할 수 없어욧", "Help Pick");
                return;
            }
            else if(input_number.Text == "?")
            {
                input_count = 1;
                input_number.Text = "1";
            }

            input_count++;
           
            input_number.Text = input_count.ToString();
        }

        //input_count--
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (input_count == 1)
            {
                //MessageBox.Show("더이상 삭제할 수 없어욧", "Help Pick");
                input_number.Text = "1";
                return;
            }
            else if(input_number.Text == "?"){
                input_number.Text = "1";
                input_count = 1;
            }
            
            input_count--;

            input_number.Text = input_count.ToString();
        }

        //선택지 수 입력
        //input number
        private void Input_btn_Click(object sender, RoutedEventArgs e)
        {
            //입력값 숫자인지 확인
             foreach (char c in input_number.Text)
             {
                if (!char.IsDigit(c))
                {                    
                    MessageBox.Show("숫자를 입력해주세요", "Help Pick");
                    input_number.Text = "";
                    return;
                }
             }

            //텍스트 int로 변환
            //다른방식 int value = int.Parse(text);
            int range_ = Convert.ToInt32(input_number.Text);

            if (10 < range_ || range_ < 2)
            {
                MessageBox.Show("선택지는 최소2개 최대 10개 까지 입력이 가능합니다.", "Help Pick");
                input_number.Text = "";
                return;
            }

            inform_stack.Visibility = Visibility.Collapsed;
            select_tab.Visibility = Visibility.Collapsed;

            //인풋 텍스트 박스 추가해주기
            input_tab.Visibility = Visibility.Visible;
            
            input_btn.Visibility = Visibility.Collapsed;

            help_btn.Visibility = Visibility.Visible;
            reset_btn.Visibility = Visibility.Visible;

            input_number.Visibility = Visibility.Collapsed;

            for(int i=0; i< range_; i++)
            {
                //입력 텍스트박스 추가
                TextBox txtbox = new TextBox();

                txtbox.Margin = new Thickness(1);
                txtbox.Width = 70;
                txtbox.FontSize = 20;
                txtbox.Background = Brushes.Ivory;
                txtbox.TextAlignment = TextAlignment.Center;
                txtbox.Name = "input" + input_count;
                
                input_tab.Children.Add(txtbox);
            }

            input_count = range_;

            input_number.Text = "?";

        }

        //텍스트박스 클릭 시 기존 입력값 사라짐
        private void Input_number_GotFocus(object sender, RoutedEventArgs e)
        {
            input_number.Text = "";
        }
        
    }
}
