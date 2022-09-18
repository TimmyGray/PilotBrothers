using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PilotBrothers.Model
{

    public class SafeKey 
    {

        bool rotate;                        //состояние ключа (горизонтальный или вертикальный)
        int x;                              //координаты в массиве
        int y;

        public bool Rotate {                //свойства, для ограничения доступа и проверки значений
            get
            {
                return rotate;
            }
            private set
            {
                if (value==false||value==true)
                {
                    rotate = value;

                }
                else
                {
                    throw new ArgumentException("Not a bool type");
                }
            }

        } 
        public int X {
            get 
            {
                return x;
            }
            
            private set 
            {
                if (value>=0||value<=5)
                {
                    x = value;
                }
            } 
        }
        public int Y
        {
            get
            {
                return y;
            }

            private set
            {
                if (value >= 0 || value <= 5)
                {
                    y = value;
                }
            }
        }

        public void ChangeRotate() //метод, имитирущий смену позиции с горизонтальной на вертикальную и наоборот
        {
            if (rotate==true)
            {
                rotate = false;
            }
            else
            {
                rotate = true;
            }
        }

        public SafeKey(int x,int y)  //конструктор ключа, с заданием рандомного положения
        {
            X = x;
            Y = y;

            Random random = new Random();
            int rnd = random.Next(0, 2);
            Console.WriteLine(rnd);
            if (rnd==0)
            {
                Rotate = false;
            }
            else
            {
                Rotate = true;
            }
        }

        
    }
}
