using System.IO;
using System;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BridgePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Brige Pattern Sample");
            CalculatePrice calculateRetailUsedCarPrice = new RetailCustomer(new CalculateUsedCarPrice());
            var retailUsedCarResult = calculateRetailUsedCarPrice.GetPrice();
            Console.WriteLine($"{retailUsedCarResult.PriceType.description()}:{retailUsedCarResult.Price}");

             CalculatePrice calculateRetailNewCarPrice = new RetailCustomer(new CalculateNewCarPrice());
            var retailNewCarResult = calculateRetailNewCarPrice.GetPrice();
            Console.WriteLine($"{retailNewCarResult.PriceType.description()}:{retailNewCarResult.Price}");


            Console.WriteLine("---------------------------------------------------------");

            CalculatePrice calculateCorporateUsedCarPrice = new CorporateCustomer(new CalculateUsedCarPrice());
            var corporateUsedCarResult = calculateCorporateUsedCarPrice.GetPrice();
            Console.WriteLine($"{corporateUsedCarResult.PriceType.description()}:{corporateUsedCarResult.Price}");

             CalculatePrice calculateCorporateNewCarPrice = new CorporateCustomer(new CalculateNewCarPrice());
            var corporateNewCarResult = calculateCorporateNewCarPrice.GetPrice();
            Console.WriteLine($"{corporateNewCarResult.PriceType.description()}:{corporateNewCarResult.Price}");
        }
        
    }
    public static class EnumExtensions{
        public static string description(this Enum value){
           var field = value.GetType().GetField( value.ToString() );
        var attributes = field.GetCustomAttributes( false );

        // Description is in a hidden Attribute class called DisplayAttribute
        // Not to be confused with DisplayNameAttribute
        dynamic displayAttribute = null;

        if (attributes.Any())
        {
            displayAttribute = attributes.ElementAt( 0 );
        }

        // return description
        return displayAttribute?.Description ?? value.ToString();
        }
    }

    

    public enum CalculateType
    {

     [Display( Name = "New", Description = "New Car Price" )]
        New,//Price for new car/UnUsed
   [Display( Name = "Used", Description = "Used Car Price" )]
        Used//Price for Used Car
    }

    public class CalculateResultModel
    {
        public CalculateType PriceType { get; set; }
        public decimal Price { get; set; }
    }

    public interface ICalculatePriceRentCar
    {
        CalculateResultModel Calculate(decimal percentageDiscount);
    }

    public class CalculateNewCarPrice : ICalculatePriceRentCar
    {
        public CalculateResultModel Calculate(decimal percentageDiscount)
        {
            var price=30M;
            return new CalculateResultModel() { Price = (price-((price/100M)*percentageDiscount)), PriceType = CalculateType.New };
        }
    }

    public class CalculateUsedCarPrice : ICalculatePriceRentCar
    {
        public CalculateResultModel Calculate(decimal percentageDiscount)
        {
            var price=25M;
            return new CalculateResultModel() { Price = (price-((price/100M)*percentageDiscount)), PriceType = CalculateType.Used };
        }
    }

    public abstract class CalculatePrice
    {
        public readonly  ICalculatePriceRentCar _calculate ;
        public CalculatePrice(ICalculatePriceRentCar calculate)
        {
            _calculate = calculate;
        }

        public abstract CalculateResultModel GetPrice();
    }

    public class RetailCustomer : CalculatePrice
    {
        public RetailCustomer(ICalculatePriceRentCar calculate) : base(calculate)
        {

        }

        public override CalculateResultModel GetPrice()
        {
            Console.WriteLine("Retail Customer");
          return  _calculate.Calculate(5);
        }
    }

    public class CorporateCustomer : CalculatePrice
    {
        public CorporateCustomer(ICalculatePriceRentCar calculate) : base(calculate)
        {

        }

        public override CalculateResultModel GetPrice()
        {
            Console.WriteLine("Corporate Customer");
           return _calculate.Calculate(10);
        }
    }

}


