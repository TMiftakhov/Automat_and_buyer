using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automat_and_buyer
{
    public interface IVendingMachine
    {
        //Автомат предоставляет меню
        Dictionary<string, uint> getPrice();
        //Положить в автомат монеты
        void addDeposit(Dictionary<uint, uint> d);
        //Узнать баланс депозита
        uint getDepositValue();
        //Выдача сдачи минимальным количеством монет
        Dictionary<uint, uint> getDelivery(ref bool success);
        //Процесс покупки и выдачи еды
        int getFood(string f);
        //Информация о банке автомата
        string showWallet();
        //Информация о наличии еды в автомате
        string showFood();
    }
    public class VendingMachine : IVendingMachine
    {
        //Указывается начальный кеш автомата
        public VendingMachine(uint monetaryCapital)
        {
            wallet = new Wallet(monetaryCapital);
            foodPrice = new Dictionary<string, uint>()//Указывается, что может быть в автомате и его цену
            {
                { "Cake", 50 },
                { "Cookie", 10 },
                { "Wafers", 30 }
            };
            foodCount = new Dictionary<string, uint>()//Указывается товар и его количество
            {
                { "Cake", 4 },
                { "Cookie", 3 },
                { "Wafers", 10 }
            };
            deposit = 0;
        }

        private Wallet wallet;//Банк автомата
        private Dictionary<string, uint> foodPrice;//Меню товара
        private Dictionary<string, uint> foodCount;//Наличие товара
        private uint deposit;//Баланс депозита

        //Посчитать сумму монет
        private uint getSum(Dictionary<uint, uint> dep)
        {
            uint sum = 0;
            foreach (var d in dep)
                sum += d.Key * d.Value;
            return sum;
        }

        //Узнать меню автомата
        public Dictionary<string, uint> getPrice()
        {
            return foodPrice;
        }

        //Добавить денег в автомат
        public void addDeposit(Dictionary<uint, uint> dep)
        {
            deposit += getSum(dep);
            wallet.setCoins(dep);
        }

        //Узнать баланс депозита
        public uint getDepositValue()
        {
            return deposit;
        }
        
        //Выдача сдачи минимальным количеством монет, если это возможно
        public Dictionary<uint, uint> getDelivery(ref bool success)
        {
            var tmp = wallet.getCoins(deposit, ref success);
            if (success)
                return tmp;
            wallet.setCoins(tmp);
            return new Dictionary<uint, uint>();
        }

        //Процесс выдачи еды
        public int getFood(string f)
        {
            if (foodCount.ContainsKey(f) && foodPrice.ContainsKey(f))//Автомат знает такой товар
                if (foodCount[f] > 0)//В автомате есть эта еда
                    if (deposit >= foodPrice[f])//Депозита достаточно для покупки
                    {
                        foodCount[f]--;
                        deposit -= foodPrice[f];
                        return 0;
                    }
                    else return -1;
                else return -2;
            else return -3;
        }

        //Информация о монетах
        public string showWallet()
        {
            return wallet.showWallet();
        }

        //Информация о еде в автомате
        public string showFood()
        {
            string tmp = "Еда в автомате:\n";
            bool b = false;
            foreach (var f in foodCount)
            {
                if (f.Value > 0)
                {
                    tmp += String.Format(" # {0} - {1} шт.\n", f.Key, f.Value);
                    b = true;
                }
            }
            tmp += String.Format("Баланс депозита: {0}\n", deposit);
            if (b) return tmp;
            return "";
        }
    }
}
