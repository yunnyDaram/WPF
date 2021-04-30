using System;
using System.Collections.Generic;
using System.Linq; //list .toList
using System.Media; //SoundPlayer
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

//System.Linq.dll

namespace BullsAndCows
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        //not use now,
        Grid display_grid = new Grid();


        //Check game state (start or not)
        //게임 시작해서 경기 중인지 확인
        public static bool IsStart = false;


        //make random numbers and process that numbers for string
        //랜덤하게 생성할 수와 랜덤 수를 문자열로 바꾸어 처리
        static int num = 0;
        static string str = "";
        static string tmp_str = "";

        int[] input_arr = new int[4];

        //get record when you strike all 4 numbers
        //4개 숫자와 자리를 모두 맞을 시 얻게되는 점수
        static int score = 0;

        //check strike and ball in what you input
        //스트라이크, 볼 판별 변수
        static int strike = 0;
        static int ball = 0;


        //now, you can only play game with 4numbers
        //랜덤 수 - 4자리 수 기본 지정 추후 변경 가능 예정
        static int random_cnt = 4;

        //you can have only 9 chances to check if the numbers you input, are correct
        //사용자가 입력 가능한 횟수
        public static int cnt = 9;

        //cnt - reverse_cnt = left count
        public static int reverse_cnt = 1;

        //not use now
        Random bg_Random = new Random();


        //collection of numbers (0-9), that can make random numbers,
        //and process the random number and input number for string
        //0~9까지 수가 있는 리스트에서 랜덤하게 뽑아 4자리의 수를 만들고,
        //그것을 문자열로 처리하기 위해 string 리스트도 만듦
        List<int> Num_range = new List<int>()
        {
            0,1,2,3,4,5,6,7,8,9
        };

        List<int> Baseball_range = new List<int>()
        {
            0,1,2,3,4,5,6,7,8,9
        };

        //랜덤 생성 문자 리스트
        List<string> Num_str = new List<string>()
        {
            "0", "1", "2", "3", "4", "5", "6", "7" , "8", "9"
        };

        List<string> Num_default_str = new List<string>()
        {
            "0", "1", "2", "3", "4", "5", "6", "7" , "8", "9"
        };


        //not use now
        //랜덤으로 출력되는 이미지
        Random rd_bg = new Random();
        static int rd_bg_num = 0;

        //not use now
        public SoundPlayer player;


        public MainWindow()
        {
            InitializeComponent();

            rd_bg_num = rd_bg.Next(0, 3);

            player = new SoundPlayer();
        }


        //game start, when the game restart, then all variables are reset
        //게임 시작 / 재시작 시 변수 초기화
        public void Reset_var()
        {
            IsStart = false;

            //the scoreboard is reset
            //전광판 리셋
            display.Children.Clear();

            //all variables used for play reset
            //볼카운트, 이닝, 입력 문자열 등 모든 게임에 사용되는 변수 값 초기화
            Baseball_range = Num_range.ToList();
            str = "";
            num = 0;
            random_cnt = 4;
            reverse_cnt = 1;
            cnt = 9;
            input_num.Text = "";
        }


        //check your input number
        //입력한 수 확인 및 볼, 스트라이크 판정 모듈
        private void Enter_Input_Click(object sender, RoutedEventArgs e)
        {
            //the game is not start, you have to click game start
            if (IsStart == false)
            {
                MessageBox.Show("경기가 시작되지 않았습니다. 하단의 경기 시작을 눌러 경기를 시작해주세요", "야구 게임");
                input_num.Text = "";
            }
            else
            {
                //숫자 입력
                if (Check_number())
                {
                    //입력 받은 문자열 변수
                    string input_s = input_num.Text;
                    int input_size = input_s.Length;

                    Array.Clear(input_arr, 0, 4);


                    //check input number size, if input > 4, return
                    //4자리 숫자만 입력 가능
                    if (input_size != 4)
                    {
                        MessageBox.Show("4자리의 숫자를 입력해주시길 바랍니다", "야구 게임");
                        input_num.Text = "";
                        return;
                    }

                    //check the same number
                    //중복 숫자 입력 방지
                    for (int i = 0; i < 4; i++)
                    {
                        input_arr[i] = (int)input_s[i];
                    }

                    int tmp = 0;

                    Check_Input();
                    if (Check_Input() == false)
                    {
                        MessageBox.Show("중복된 숫자를 입력하셨습니다", "야구 게임");
                    }
                    else
                    {
                        //입력 한 수 = 생성된 난 수 같으면 점수 획득 후 경기 종료
                        if (answer.Text == input_num.Text)
                        {
                            IsStart = false;
                            //한번에 맞힘
                            if (cnt == 9)
                            {
                                //경기 끝 점수 획득
                                score += 100;
                                score_t.Text = "점수: " + score.ToString() + " 점";
                                MessageBox.Show("이요르~", "경기 종료");
                            }
                            else
                            {
                                //경기 끝 점수 획득 (점수 계산 = 남은 횟수 * 10)
                                score += cnt * 10;
                                score_t.Text = "점수: " + score.ToString() + " 점";

                            }
                            MessageBox.Show(reverse_cnt + "년 만에 두산 베어스 우승", "경기 종료");
                            Reset_var();


                            //
                        }
                        else if (cnt == 0) // 게임 끝
                        {
                            User_lose();
                        }
                        else //ball, strike 판정
                        {
                            string ans_str = answer.Text;

                            for (int i = 0; i < 4; i++)
                            {
                                string comp = input_s[i].ToString();

                                if (!Num_str.Contains(comp))
                                {

                                    if (input_s[i] == str[i])
                                    {
                                        strike++;
                                    }
                                    else
                                    {
                                        ball++;
                                    }
                                }
                            }
                            ball_count_t.Text = "볼 카운트는 : " + strike + "S  " + ball + " B  ";
                            //히스토리 추가

                            //0회 이름
                            string history = ("inning_" + reverse_cnt).ToString();

                            //입력 이름
                            string input_str = ("input_" + reverse_cnt).ToString();

                            //결과 이름
                            string str_result_name = "result" + reverse_cnt;


                            //write scoreboard
                            //입력한 숫자 확인 후 전광판 기록
                            StackPanel stp = new StackPanel();

                            TextBlock inning = new TextBlock();
                            inning.Name = history;
                            inning.Text = reverse_cnt + " 회";

                            TextBlock input = new TextBlock();
                            input.Name = input_str;
                            input.Text = input_num.Text;

                            TextBlock result = new TextBlock();
                            result.Name = str_result_name;
                            result.Text = strike + "S  " + ball + " B  ";

                            //set color for counting strike, ball
                            //볼카운트 UI 색 지정
                            inning.Foreground = new SolidColorBrush(Colors.Ivory);
                            input.Foreground = new SolidColorBrush(Colors.Ivory);
                            result.Foreground = new SolidColorBrush(Colors.Ivory);
                            inning.FontSize = 40;
                            input.FontSize = 25;
                            result.FontSize = 25;

                            stp.Children.Add(inning);
                            stp.Children.Add(input);
                            stp.Children.Add(result);

                            //display_grid.Children.Add(stp);

                            display.Children.Add(stp);

                            //reset all variables about count strike, ball, when finish checking your input numbers
                            //볼카운트 판별 후 다음 값을 위해 다시 관련 변수 값들 초기화
                            strike = 0;
                            ball = 0;
                            input_num.Text = "";
                            reverse_cnt++;
                            cnt--;
                        }
                    }
                }
                else
                {
                    //return;
                    input_num.Text = "";
                }

            }
        }

        //game over, you lose
        //게임 종료 - 영봉패
        public void User_lose()
        {
            //기회 다 씀
            //리셋
            if (MessageBox.Show("완봉패 ㅋㅋ", "경기 종료", MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                if (MessageBox.Show("점수를 깎고 정답을 확인하시겠습니까?", "정답 확인", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    MessageBox.Show("정답은 " + str + " 입니다");

                    score -= 50;
                    score_t.Text = "점수: " + score.ToString() + " 점";
                }
            }
            Reset_var();

            //브라보 마이 라이프넣기
        }

        //check the same number what you enter
        //사용자 입력값 중 중복 값 확인
        public bool Check_Input()
        {
            //비교 임시 변수
            int tmp = 0;

            if (IsStart == false)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    tmp = input_arr[i];
                    test.Text += "tmp : " + tmp.ToString() + "\n";
                    for (int j = i + 1; j < 4; j++)
                    {
                        //test.Text += "input_arr["+j+"] 값 :"+input_arr[j] + "\n";
                        if (input_arr[i] == input_arr[j])
                        {
                            //MessageBox.Show("중복된 숫자를 입력하셨습니다");
                            input_num.Text = "";
                            return false;
                        }
                    }
                }
                return true;
            }
        }


        //game start button
        //게임 시작 - 난수 생성 모듈
        private void Start_Btn_Click(object sender, RoutedEventArgs e)
        {
            Game_Start();
        }

        //make random numbers for play
        //난수 생성 함수
        public void Game_Start()
        {
            //이미 경기 중
            if (IsStart)
            {
                if (MessageBox.Show("게임이 진행 중입니다. 취소하시고 다시 하시겠습니까?", "경기 종료", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    //현재 경기 취소 후 다시 시작
                    IsStart = false;

                    Reset_var();
                }
                else
                {
                    //현재 경기 진행
                }
            }
            else //경기중이 아님
            {
                IsStart = true;

                display.Children.Clear();

                //리셋
                Baseball_range = Num_range.ToList();
                Num_str = Num_default_str.ToList();
                str = "";
                num = 0;
                random_cnt = 4;
                reverse_cnt = 1;
                cnt = 9;

                Random rnd = new Random();
                int begin = 0;
                bg_img.Visibility = Visibility.Visible;
                //이미지 넣기

                while (random_cnt > 0)
                {
                    int random_size = Baseball_range.Count() - 1;
                    num = rnd.Next(begin, random_size);

                    int val = Baseball_range[num];
                    Baseball_range.RemoveAt(num);
                    str += val.ToString();

                    //string 제거 - 나중 입력 시 비교 string list
                    tmp_str += Num_str[num];
                    Num_str.RemoveAt(num);

                    //random number = target number
                    //test.Text = tmp_str;
                    random_cnt--;
                }

                answer.Text = str;
            }
        }


        //just only can png image file, register image
        //이미지 등록하기
        //png만 가능
        private void Img_btn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if (openDialog.ShowDialog() == true)
            {
                if (File.Exists(openDialog.FileName))
                {
                    Stream imageStreamSource = new FileStream(openDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    PngBitmapDecoder decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    BitmapSource bitmapSource = decoder.Frames[0];


                    //img.Source = bitmapSource;

                }
            }
        }


        //check the input is a number
        //입력값 숫자인지 확인
        public bool Check_number()
        {
            foreach (char c in input_num.Text)
            {
                if (!char.IsDigit(c))
                {
                    //e.Handled = true;
                    MessageBox.Show("숫자를 입력해주세요", "야구 게임");
                    return false;
                }
            }

            return true;
        }


        //you can input use a 'enter' key
        //엔터 키로 입력 가능
        private void Input_num_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Enter_Input_Click(sender, e);
            }
        }

        //play a bgm
        private void Play_bgm_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            int rnd_ = rnd.Next(1, 3);

            //random number = target number
            test.Text += rnd_.ToString() + "\n";

            var rnd_bgm = Properties.Resources.ResourceManager.GetStream("_" + rnd_);
            System.Media.SoundPlayer sp = new System.Media.SoundPlayer(rnd_bgm);
            //player = SoundPlayerAction(rnd_bgm);
            sp.Play();
        }


        //stop the bgm
        private void Stop_bgm_Click(object sender, RoutedEventArgs e)
        {
            SoundPlayer sp = new SoundPlayer();
            sp.Stop();
        }

    }
}
