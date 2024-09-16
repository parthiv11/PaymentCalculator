using PaymentCalculator.Wpf;
using VoidCore.Finance;
using WpfPilot;

namespace PaymentCalculator.UITest
{
    public class UITests
    {
        [Fact]
        public void HandlesNormalWorkflow()
        {
            // Launch the `PaymentCalculator` app.
            // Make sure you built the app before running the tests.
            using var appDriver = AppDriver.Launch(@"..\..\..\..\..\src\PaymentCalculator.Wpf\bin\Debug\net7.0-windows\PaymentCalculator.exe");

            // Fill out the asset cost.
            appDriver.GetElement(x => x["Name"] == "AssetCostTextBox")
                .SetProperty("Text", "") // Clear the text box.
                .Type("$10000");

            // Fill out the down payment.
            appDriver.GetElement(x => x["Name"] == "DownPaymentTextBox")
                .SetProperty("Text", "")
                .Type("$1000");

            // Fill out the interest rate.
            appDriver.GetElement(x => x["Name"] == "AnnualInterestRateTextBox")
                .SetProperty("Text", "")
                .Type("3%");

            // Fill out the years.
            appDriver.GetElement(x => x["Name"] == "YearsTextBox")
                .SetProperty("Text", "")
                .Type("5");

            // Select a period frequency.
            appDriver.GetElement(x => x["Name"] == "PeriodsPerYearComboBox")
                .SetProperty("SelectedIndex", 1);

            // Fill out the escrow payment.
            appDriver.GetElement(x => x["Name"] == "EscrowPerPeriodTextBox")
                .SetProperty("Text", "")
                .Type("$100");

            // Calculate the result.
            appDriver.GetElement(x => x["Name"] == "CalcButton").Click();

            // Verify the results.
            var lastRowResult = appDriver.GetElement(x => x.TypeName == nameof(MainWindow))
                .Invoke<MainWindow, List<AmortizationPeriod>>(x => ((LoanViewModel)x.DataContext).Schedule.ToList())
                .Last();
            Assert.Equal(0.0m, lastRowResult.BalanceLeft);
            Assert.Equal(0.0m, lastRowResult.InterestPayment);
            Assert.Equal(20, lastRowResult.PeriodNumber);
            Assert.Equal(450.0m, lastRowResult.PrincipalPayment);
        }
    }
}
