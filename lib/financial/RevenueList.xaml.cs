﻿using intnet22.lib.general;

namespace intnet22.lib.financial
{
    /// <summary>
    /// Interaction logic 
    /// </summary>
    public class RevenueList : ExpenseList
    {

        //
        protected override string MyOperacao()
        {
            //
            this.Title = "Receitas";
            return "credito";
        }

        //
        // protected override void myTitle()
        // {
            //
            // return "Despesas";
        // }



    }
}