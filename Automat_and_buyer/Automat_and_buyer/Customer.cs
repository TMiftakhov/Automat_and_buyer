using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automat_and_buyer
{
    public interface ICustomer
    {
        //Подойти покупателю к указанному автомату
        void setVendingMachine(IVendingMachine vm);
        //Процесс покупки еды
        int buyFood(string f);
        //Информация о кошельке покупателя
        string showWallet();
        //Еда у покупателя
        string showFood();
    }
    public class Customer : ICustomer
    {
        //Указывается начальная сумма денег у покупателя
        public Customer(uint initialSum)
        {
            wallet = new Wallet(initialSum);//Кошелек
            foodCount = new Dictionary<string, uint>();
        }

        private IVendingMachine vendingMachine;//Торговый автомат
        private Wallet wallet;//Кошелек
        private Dictionary<string, uint> foodPrice;//Цена продуктов в автомате
        private Dictionary<string, uint> foodCount;//Продукты у клиента

        //Подойти покупателю к указанному автомату
        public void setVendingMachine(IVendingMachine vm)
        {
            vendingMachine = vm;
            foodPrice = vendingMachine.getPrice();//Узнает меню и цены у автомата
        }

        public int buyFood(string f)//Процесс покупки еды
        {
            if (vendingMachine == null) return 4;//Покупатель не у автомата
            if (!(foodPrice.ContainsKey(f))) return 3;//В автомете нет желаемой еды и никогда не было
            if ((wallet.getMoneyCount() + vendingMachine.getDepositValue()) < foodPrice[f]) return 2;//Покупателю не хватает денег на еду
            bool b = true;
            vendingMachine.addDeposit(wallet.getCoins(foodPrice[f] - vendingMachine.getDepositValue(), ref b));//Прибавить к депозиту достаточное количество денег
            int i = vendingMachine.getFood(f);//Купить и забрать еду
            if (i == 0)//Получает еду
                if (!(foodCount.ContainsKey(f)))
                    foodCount.Add(f, 1);
                else foodCount[f]++;
            bool delivery = false;
            vendingMachine.getDelivery(ref delivery);//Забрать сдачу
            if (!delivery && i == 0) return 1;
            return i;
        }

        //Информация о кошельке покупателя
        public string showWallet()
        {
            return wallet.showWallet();
        }

        //Еда у покупателя
        public string showFood()
        {
            string tmp = "Еда у покупателя:\n";
            bool b = false;
            foreach (var f in foodCount)
            {
                if (f.Value > 0)
                {
                    tmp += String.Format(" # {0} - {1} шт.\n", f.Key, f.Value);
                    b = true;
                }
            }
            if (b) return tmp;
            return "";
        }
    }
}
