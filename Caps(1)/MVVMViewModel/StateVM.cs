using Caps_1_.MVVMModel;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Caps_1_.MVVMModel.MyDataModel;

namespace Caps_1_.MVVMViewModel
{
    public class StateVM : BindableBase
    {
        private MyDataModel _dataModel;


        public StateVM()
        {
            _dataModel = MyDataModel.Instance;
            CONV_1 = _dataModel.CONV_1;
            CONV_2 = _dataModel.CONV_2;
            STOPPER_1 = _dataModel.STOPPER_1;
            STOPPER_2 = _dataModel.STOPPER_2;
            OUT_CYL = _dataModel.OUT_CYL;
            DISPOSE_CYL = _dataModel.DISPOSE_CYL;
            GAN_ARM = _dataModel.GAN_ARM;
            GAN_ARM_ROT = _dataModel.GAN_ARM_ROT;
            GAN_ARM_GIP = _dataModel.GAN_ARM_GIP;
            X_AXIS_MOTOR = _dataModel.X_AXIS_MOTOR;
            Y_AXIS_MOTOR = _dataModel.Y_AXIS_MOTOR;

            _dataModel.PropertyChanged += DataModel_PropertyChanged;
        }
        private void DataModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_dataModel.CONV_1))
            {
                CONV_1 = _dataModel.CONV_1;
            }
            else if (e.PropertyName == nameof(_dataModel.CONV_2)) { CONV_2 = _dataModel.CONV_2; }

            else if (e.PropertyName == nameof(_dataModel.STOPPER_1))
            {
                STOPPER_1 = _dataModel.STOPPER_1;
            }
            else if (e.PropertyName == nameof(_dataModel.STOPPER_2))
            {
                STOPPER_2 = _dataModel.STOPPER_2;
            }
            else if (e.PropertyName == nameof(_dataModel.OUT_CYL))
            {
                OUT_CYL = _dataModel.OUT_CYL;
            }
            else if (e.PropertyName == nameof(_dataModel.DISPOSE_CYL))
            {
                DISPOSE_CYL = _dataModel.DISPOSE_CYL;
            }
            else if (e.PropertyName == nameof(_dataModel.GAN_ARM))
            {
                GAN_ARM = _dataModel.GAN_ARM;
            }
            else if (e.PropertyName == nameof(_dataModel.GAN_ARM_ROT))
            {
                GAN_ARM_ROT = _dataModel.GAN_ARM_ROT;
            }
            else if (e.PropertyName == nameof(_dataModel.GAN_ARM_GIP))
            {
                GAN_ARM_GIP = _dataModel.GAN_ARM_GIP;
            }
            else if (e.PropertyName == nameof(_dataModel.X_AXIS_MOTOR))
            {
                X_AXIS_MOTOR = _dataModel.X_AXIS_MOTOR;

            }
            else if (e.PropertyName == nameof(_dataModel.Y_AXIS_MOTOR))
            {
                Y_AXIS_MOTOR = _dataModel.Y_AXIS_MOTOR;
            }
        }




        private string _conv_1, _conv_2;

        public string CONV_1
        {
            get { return _conv_1; }
            set { _conv_1 = value; }
        }
        public string CONV_2
        {
            get { return _conv_2; }
            set { _conv_2 = value; }
        }


        private string _stopper1, _stopper2, _outCyl, _disposeCyl, _ganArm, _ganArmRot, _ganArmGip;
        private int x_motor, y_motor;

        public string STOPPER_1
        {
            get { return _stopper1; }
            set { SetProperty(ref _stopper1, value); }
        }

        public string STOPPER_2
        {
            get { return _stopper2; }
            set { SetProperty(ref _stopper2, value); }
        }

        public string OUT_CYL
        {
            get { return _outCyl; }
            set { SetProperty(ref _outCyl, value); }
        }

        public string DISPOSE_CYL
        {
            get { return _disposeCyl; }
            set { SetProperty(ref _disposeCyl, value); }
        }

        public string GAN_ARM
        {
            get { return _ganArm; }
            set { SetProperty(ref _ganArm, value); }
        }

        public string GAN_ARM_ROT
        {
            get { return _ganArmRot; }
            set { SetProperty(ref _ganArmRot, value); }
        }

        public string GAN_ARM_GIP
        {
            get { return _ganArmGip; }
            set { SetProperty(ref _ganArmGip, value); }
        }
        
        public int X_AXIS_MOTOR
        {
            get { return x_motor; }
            set { SetProperty(ref x_motor, value); }
        }
        public int Y_AXIS_MOTOR
        {
            get { return y_motor; }
            set { SetProperty(ref y_motor, value); }
        }
    }
}
