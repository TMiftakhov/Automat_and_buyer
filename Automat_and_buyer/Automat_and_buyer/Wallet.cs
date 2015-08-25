using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automat_and_buyer
{
    public class Wallet
    {
        //Указывается сумма денег в кошельке
        public Wallet(uint sum)
        {
            nominals = new uint[] { 1, 2, 5, 10 };//Указываются существующие номиналы
            generateCoins(sum);//Генерация монет
        }

        private Dictionary<uint, uint> coins = new Dictionary<uint,uint>();//Монеты в кошельке
        private uint[] nominals;//Номиналы

        private void generateCoins(uint sum)//Генерация монет
        {
            Array.Sort(nominals, (x, y) => -x.CompareTo(y));//Сортировка
            foreach (var n in nominals)
            {
                if (nominals.Last() == n)
                {
                    coins.Add(n, sum / n);
                    continue;
                }
                uint tmp = sum - sum / 2;
                coins.Add(n, tmp / n);
                sum += tmp % n - tmp;
            }
        }

        //Получить монеты из кошелька указанной суммы минимальным количеством
        public Dictionary<uint, uint> getCoins(uint sum, ref bool success)
        {
            if (sum > this.getMoneyCount())//Требуемая сумма больше суммы монет в кошельке
            {
                success = false;
                return null;
            }
            Dictionary<uint, uint> tmp = new Dictionary<uint, uint>();
            uint residuals = sum;//Остаток
            uint total = 0;//Набранная суммма монет
            
            foreach (var n in nominals)//Номиналы уже отсортированы при генерации
            {
                if ((n * coins[n]) >= residuals)//Текущего номинала не меньше остатка
                {
                    coins[n] -= residuals / n;
                    if (!(tmp.ContainsKey(n))) 
                        tmp.Add(n, 0);
                    tmp[n] += residuals / n;
                    total += residuals;
                    residuals %= n;
                    if (residuals == 0) break;
                    continue;
                }
                else//Текущиего номинала меньше остатка
                {
                    residuals -= n * coins[n];
                    if (!(tmp.ContainsKey(n))) 
                        tmp.Add(n, 0);
                    tmp[n] += coins[n];
                    total += n * coins[n];
                    coins[n] = 0;
                }
            }
            if (total == sum) success = true;//Сумма выданных монет точная
            else success = false;
            return tmp;
        }

        //Положить монеты в кошелек
        public void setCoins(Dictionary<uint, uint> co)
        {
            foreach (var c in co)
            {
                if (!(coins.ContainsKey(c.Key)))
                    coins.Add(c.Key, c.Value);
                else coins[c.Key] += c.Value;
            }
        }
        
        //Узнать сумму монет в кошельке
        public uint getMoneyCount()
        {
            uint sum = 0;
            foreach (var d in coins)
                sum += d.Key * d.Value;
            return sum;
        }

        //Узнать информацию о монетах в кошельке
        public string showWallet()
        {
            string tmp = getMoneyCount().ToString() + '\n';
            tmp += coins.Select(x => "[" + x.Key + " - " + x.Value + "]\n").Aggregate((s1, s2) => s1 + s2);
            return tmp;
        }
    }
}
