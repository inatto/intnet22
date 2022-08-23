using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using intnet22.lib.general;
using intnet22.lib.general.wpf;
using MySql.Data.MySqlClient;

namespace intnet22.lib.admin;

public class GeneralForm : GeneralWindow
{
    //
    protected readonly MySqlConnection Conn;
    protected readonly string? Action;
    protected readonly long? Id;
    protected Vo? Vo;

    protected GeneralForm(string action, long id)
    {
        //
        Action = action ?? throw new ArgumentNullException(nameof(action));
        Id = id;

        //
        if (Action == Constants.Update && Id == 0)
            throw new Exception("id nao pode ser zero ao alterar");

        //
        Conn = MySqlModule.Connectt();
    }

    protected void Persist()
    {
        //
        try
        {
            switch (Action)
            {
                case Constants.Insert:
                    Insert();
                    break;
                // case Constants.Update when CheckRemover.IsChecked == true:
                // Delete();
                // break;
                case Constants.Update:
                    Update();
                    break;
            }

            //
            ((IGrid)Owner).LoadGrid();
            this.Close();
        }
        catch (ValidationException e)
        {
            MessageBox.Show(e.Message, "Validação", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        catch (MySqlException e)
        {
            MessageBox.Show(e.Message, "Conexão", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Integridade", MessageBoxButton.OK, MessageBoxImage.Error);
            Environment.Exit(1);
        }
    }

    protected virtual void Insert()
    {
    }

    protected virtual void Update()
    {
    }
}