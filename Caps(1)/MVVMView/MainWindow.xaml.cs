using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.WpfExtensions;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Caps_1_.MVVMModel;
using System.Windows.Markup;
using System.Threading;
using static Caps_1_.MVVMModel.MyDataModel;
using System.Windows.Forms;

namespace Caps_1_.MVVMView
{
    public partial class MainWindow : System.Windows.Window
    {
        VideoCapture cam;
        Mat frame;
        DispatcherTimer timer;
        bool isInitCam, isInitTimer;
        bool isPaused;
        bool isInferenceing = false;


        public MainWindow()
        {
            InitializeComponent();

            // KeyDown 이벤트 핸들러를 등록합니다.
            //this.KeyDown += MainWindow_KeyDown;

            timer = new DispatcherTimer();
        }

        private void DataSetDateTime()
        {
            k_Time.Text = DateTime.Now.ToShortTimeString();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DataSetDateTime();

            //추론 시작추론 받고 추론 시작
            if (MyDataModel.Instance.ReadCoil(1100, 1) && !isInferenceing)
            {
                isInferenceing = true;


                MyDataModel.Instance.WriteCoil(104, true);
                InferenceAndDisplayResult();
            }

            if (cam != null && cam.IsOpened())
            {
                cam.Read(frame);
                CamImage.Source = WriteableBitmapConverter.ToWriteableBitmap(frame);
                //MessageBox.Show($"{MyDataModel.Instance.ReadCoil(1100, 1)}");
               
            }
        }


        private void Home_click1(object sender, RoutedEventArgs e)
        {
            // Mode_ToggleBtn이 체크된 상태일 때
            if (Mode_ToggleBtn.IsChecked ?? false)
            {
                isInitCam = InitCamera();
                isInitTimer = InitTimer(10);

                if (isInitTimer && isInitCam)
                {
                    timer.Start();
                }
                Main.Content = new Home();
            }
            else
            {
                // Mode_ToggleBtn이 체크되지 않은 상태일 때 Stimulation 페이지로 전환
                NavigateToPage(new Stimulation());
            }
        }


        private void Setting_click1(object sender, RoutedEventArgs e)
        {
            if (Mode_ToggleBtn.IsChecked ?? true)
            {
                NavigateToPage(new Setting());
            }
            else
            {
                NavigateToPage(new ModeRestrictedPage());
            }
        }

        private void Data_click1(object sender, RoutedEventArgs e)
        {
            if (Mode_ToggleBtn.IsChecked ?? true)
            {
                NavigateToPage(new Data());
            }
            else
            {
                NavigateToPage(new ModeRestrictedPage());
            }
        }

        private void State_click1(object sender, RoutedEventArgs e)
        {
            if (Mode_ToggleBtn.IsChecked ?? true)
            {
                NavigateToPage(new State());
            }
            else
            {
                NavigateToPage(new ModeRestrictedPage());
            }
        }

        private void Log_click1(object sender, RoutedEventArgs e)
        {
            if (Mode_ToggleBtn.IsChecked ?? true)
            {
                NavigateToPage(new Log());
            }
            else
            {
                NavigateToPage(new ModeRestrictedPage());
            }
        }

        private async void AutoStart_click1(object sender, RoutedEventArgs e)
        {
            // 첫 번째 명령 실행
            MyDataModel.Instance.WriteCoil(102, true);

            // 1초(1000ms) 대기 (필요에 따라 시간을 변경할 수 있습니다)
            await Task.Delay(1000);

            // 두 번째 명령 실행
            MyDataModel.Instance.WriteCoil(102, false);
        }
        private async void AutoStop_click1(object sender, RoutedEventArgs e)
        {
            // 첫 번째 명령 실행
            MyDataModel.Instance.WriteCoil(101, true);

            // 1초(1000ms) 대기 (필요에 따라 시간을 변경할 수 있습니다)
            await Task.Delay(1000);

            // 두 번째 명령 실행
            MyDataModel.Instance.WriteCoil(101, false);
        }







        private void ViewLogButton_Click(object sender, RoutedEventArgs e)
        {
            LogBtn.IsChecked = true;
            NavigateToPage(new Log());
        }



        // 공통된 페이지 전환 메서드
        private void NavigateToPage(object page)
        {
            UnloadCamResources();
            Main.Content = page;
        }

        private void Mode_ToggleBtn_Checked(object sender, RoutedEventArgs e)
        {
            MyDataModel.Instance.WriteRegister(1,1);
            // Mode_ToggleBtn이 체크되었을 때 (true) 수행할 작업
            if (Mode_ToggleBtn.IsChecked ?? true)
            {
                // 기존 페이지 전환 로직
                if (HomeBtn.IsChecked ?? false)
                {
                    Home_click1(sender, e);
                }
                else if (SettingBtn.IsChecked ?? false)
                {
                    Setting_click1(sender, e);
                }
                else if (DataBtn.IsChecked ?? false)
                {
                    Data_click1(sender, e);
                }
                else if (StateBtn.IsChecked ?? false)
                {
                    State_click1(sender, e);
                }
                else if (LogBtn.IsChecked ?? false)
                {
                    Log_click1(sender, e);
                }

                // Mode_ToggleBtn이 체크된 상태일 때 UI 변경
                Mode_ToggleBtn.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                Mode_changeBtn.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#4957E5"));

                LinearGradientBrush gradientBrush = new LinearGradientBrush
                {
                    StartPoint = new System.Windows.Point(0, -0.5),
                    EndPoint = new System.Windows.Point(0.7, 0.6)
                };

                gradientBrush.GradientStops.Add(new GradientStop((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FBFCFE"), 0.0));
                gradientBrush.GradientStops.Add(new GradientStop((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#D1E2FA"), 0.25));
                gradientBrush.GradientStops.Add(new GradientStop((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#B1CCF0"), 0.5));
                gradientBrush.GradientStops.Add(new GradientStop((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#6DA6FA"), 0.7));
                gradientBrush.GradientStops.Add(new GradientStop((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#EAF2FD"), 1));
                out_gird.Background = gradientBrush;
            }
        }
        private void Mode_ToggleBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            MyDataModel.Instance.WriteRegister(1,2);

            // Mode_ToggleBtn이 체크되지 않았을 때 (false) 수행할 작업
            if (!(Mode_ToggleBtn.IsChecked ?? false))
            {
                // 페이지 전환 로직 수정
                if (HomeBtn.IsChecked ?? false)
                {
                    NavigateToPage(new Stimulation());
                }
                else if (SettingBtn.IsChecked ?? false)
                {
                    NavigateToPage(new ModeRestrictedPage());
                }
                else if (DataBtn.IsChecked ?? false)
                {
                    NavigateToPage(new ModeRestrictedPage());
                }
                else if (StateBtn.IsChecked ?? false)
                {
                    NavigateToPage(new ModeRestrictedPage());
                }
                else if (LogBtn.IsChecked ?? false)
                {
                    NavigateToPage(new ModeRestrictedPage());
                }
                else
                {
                    // Default action if no specific button is checked
                    NavigateToPage(new Stimulation());
                }

                // Mode_ToggleBtn이 체크되지 않은 상태일 때 UI 변경
                Mode_ToggleBtn.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                Mode_changeBtn.Background = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF7B7BE8"));

                LinearGradientBrush gradientBrush = new LinearGradientBrush
                {
                    StartPoint = new System.Windows.Point(0, -0.5),
                    EndPoint = new System.Windows.Point(0.7, 0.6)
                };

                gradientBrush.GradientStops.Add(new GradientStop((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#E6E6F0"), 0.0));
                gradientBrush.GradientStops.Add(new GradientStop((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFEFDFF"), 0.25));
                gradientBrush.GradientStops.Add(new GradientStop((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFCECEFA"), 0.45));
                gradientBrush.GradientStops.Add(new GradientStop((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFCACAF7"), 0.7));
                gradientBrush.GradientStops.Add(new GradientStop((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFF4F1F9"), 1));
                out_gird.Background = gradientBrush;
            }
        }



        #region 카메라 키는 코드


        private void UnloadCamResources()
        {
            if (cam != null)
            {
                if (cam.IsOpened())
                {
                    cam.Release();
                }
                cam.Dispose();
                cam = null;
            }
            frame?.Dispose();
            frame = null;

        }


        private bool InitTimer(double intervalMs)
        {
            try
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(intervalMs);
                timer.Tick += Timer_Tick;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool InitCamera()
        {
            while (true) // 무한 루프
            {
                try
                {
                    cam = new VideoCapture(0); // 카메라 초기화 시도
                    if (!cam.IsOpened())
                    {
                        // 1초 대기
                        System.Threading.Thread.Sleep(1000);
                        continue; // 다음 루프로 이동하여 다시 시도
                    }

                    // 해상도 설정
                    cam.Set(VideoCaptureProperties.FrameWidth, 299); // 너비 설정
                    cam.Set(VideoCaptureProperties.FrameHeight, 299); // 높이 설정

                    frame = new Mat(); // Mat 객체 초기화
                    return true; // 성공적으로 초기화되면 true 반환
                }
                catch (ArgumentNullException ex) // 값이 null일 경우
                {
                    System.Windows.Forms.MessageBox.Show($"카메라 초기화 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false; // 오류 발생 시 false 반환
                }
                catch (Exception ex) // 기타 예외 처리
                {
                    System.Windows.Forms.MessageBox.Show($"카메라 초기화 중 알 수 없는 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false; // 오류 발생 시 false 반환
                }
            }
        }

        // 스페이스바를 눌렀을 때 호출되는 이벤트 핸들러

        /*private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (!isPaused)
                {
                    timer.Stop(); // 타이머 일시 정지
                    isPaused = true;

                    // 프레임을 추론하고 결과를 출력합니다.
                    InferenceAndDisplayResult();

                    // 1초 후에 타이머 재시작
                    DispatcherTimer restartTimer = new DispatcherTimer();
                    restartTimer.Interval = TimeSpan.FromMilliseconds(1000);
                    restartTimer.Tick += (s, args) =>
                    {
                        restartTimer.Stop();
                        timer.Start();
                        isPaused = false;

                        // WriteableBitmapConverter를 사용하여 Mat 객체를 WriteableBitmap으로 변환합니다.
                        CamImage.Source = WriteableBitmapConverter.ToWriteableBitmap(frame);
                    };
                    restartTimer.Start();
                }
            }
        }*/

        // 이미지를 0과 1 사이의 값으로 정규화하는 메서드
        private float[] NormalizeImage(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            List<float> normalizedImage = new List<float>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    System.Drawing.Color pixelColor = image.GetPixel(x, y);
                    // 각 채널 값을 0과 1 사이의 값으로 정규화
                    normalizedImage.Add((float)pixelColor.R / 255.0f);
                    normalizedImage.Add((float)pixelColor.G / 255.0f);
                    normalizedImage.Add((float)pixelColor.B / 255.0f);
                }
            }

            return normalizedImage.ToArray();
        }
        // 프레임을 추론하고 결과를 출력하는 메서드
        private async void InferenceAndDisplayResult()
        {
            try
            {
                byte[] imageData;
                using (MemoryStream ms = new MemoryStream())
                {
                    frame.ToBitmap().Save(ms, ImageFormat.Bmp);
                    imageData = ms.ToArray();
                }

                using (System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(imageData)))
                {
                    int targetWidth = 299;
                    int targetHeight = 299;
                    using (Bitmap resizedImage = new Bitmap(image, targetWidth, targetHeight))
                    {
                        float[] imageDataNormalized = NormalizeImage(resizedImage);

                        string modelPath = "model.onnx";
                        using (var session = new InferenceSession(modelPath))
                        {
                            var inputTensor = new DenseTensor<float>(imageDataNormalized, new int[] { 1, targetHeight, targetWidth, 3 });
                            var inputs = new NamedOnnxValue[] { NamedOnnxValue.CreateFromTensor<float>("input_1", inputTensor) };
                            using (var results = session.Run(inputs))
                            {
                                var outputTensor = results.First().AsTensor<float>();

                                var predictedClassIndex = outputTensor.ToArray().ToList().IndexOf(outputTensor.Max());
                                var classNames = new Dictionary<int, string>
                        {
                            {0, "chinese_cabbage"},
                            {1, "garlic"},
                            {2, "onion"},
                            {3, "pepper"},
                            {4, "radish"}
                        };
                                var predictedClassName = classNames[predictedClassIndex];
                                var predictedProbability = Math.Round(outputTensor.Max() * 100, 3);
                                var A_Storage_number_select = MyDataModel.Instance.AddStorage(predictedClassName);

                                ResultTextBox.Text = $"가장 높은 확률을 가지는 클래스: {predictedClassName}\n예측 확률(%): {predictedProbability}";

                                if (A_Storage_number_select >= 0)
                                {
                                   
                                    MyDataModel.Instance.WriteRegister(3, (ushort)A_Storage_number_select);//몇번 박스에 넣을껀지
                                    MyDataModel.Instance.WriteCoil(103, true);//딥러닝 완료 신호

                                    await Task.Delay(1000); // 1초 대기

                                    MyDataModel.Instance.WriteCoil(103, false);//딥러닝 완료 신호 끄기

                                    var inventoryItem = new InventoryItem()
                                    {
                                        LogType = "입고 시퀀스",
                                        LogContent = predictedClassName + " 상품이 입고됩니다.",
                                        TargetLocation = A_Storage_number_select.ToString() + "번 창고",
                                        LogTime = DateTime.Now,
                                        Notes = predictedClassIndex == MyDataModel.Instance.ReadRegister(5, 1)?  "-" : "!경고! 예정된 입고와 불일치"
                                    };


                                    MyDataModel.Instance.AddLogItem(inventoryItem);
                                    MyDataModel.Instance.WriteCoil(104, false);

                                    // 9초 대기 후 isInferenceing을 false로 설정하여 다시 추론 가능하도록 만듦
                                    await Task.Delay(9000); 
                                    isInferenceing = false;
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("Container is full!!");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"추론 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        #endregion

    }
}
