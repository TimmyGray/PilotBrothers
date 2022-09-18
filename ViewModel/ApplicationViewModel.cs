using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using PilotBrothers.Model;


namespace PilotBrothers.ViewModel
{
    public class ApplicationViewModel
    {
        public ObservableCollection<SafeKey> SafeKeys { get; set; } //коллекция всех ключей задействованных в игре

        public bool RotateKeys(int x,int y)                         //метод отвечающий за основную логику. вызываем
                                                                    //метод поворота у каждого ключа, координаты
                                                                    //которого подходят. после этого проходимся по
                                                                    //коллекции и сравниваем положение первого
                                                                    //ключа с остальными, добавляем в новую коллекцию
                                                                    //ключи, пока они удовлетворяют условию. После этого
                                                                    //сравниваем количество ключей в новой коллекции
                                                                    //с основной коллекцией. Если оно ровно, то сейф
                                                                    //открыт и возвращается true
        {
            foreach (var key in SafeKeys)
            {
                if (key.X==x||key.Y==y)
                {
                    key.ChangeRotate();
                }
               
            }

            var samekeys = SafeKeys.TakeWhile(k => k.Rotate == SafeKeys[0].Rotate);
            if (samekeys.Count()==SafeKeys.Count)
            {
                return true;
            }
            return false;
        }

        public ApplicationViewModel()
        {
            SafeKeys = new ObservableCollection<SafeKey>();
        }

    }
}
