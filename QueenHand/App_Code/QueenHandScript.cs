using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

/// <summary>
/// Переменная скрипта QH
/// </summary>
 public class QHSVariable
{
    private string name;
    private string value;

     /// <summary>
     /// 
     /// </summary>
     /// <param name="name">имя переменной</param>
     /// <param name="value">значение переменной</param>
    public QHSVariable(string name, string value)
    {
        this.name = name;
        this.value = value;
    }

     /// <summary>
     /// Имя переменной 
     /// </summary>
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    /// <summary>
    /// Значение переменной
    /// </summary>
    public string Value
    {
        get { return value; }
        set { this.value = value; }
    }

  
}

/// <summary>
/// Правило скрипта QH
/// </summary>
 public class QHSRule
 {
     private string letterName;
     private double leftDeltaX, rightDeltaX, deltaY, scale;
     private bool isValid;
     List<string> onlyBefore = new List<string>();
     List<string> onlyAfter = new List<string>();
     List<string> onlyNotBefore = new List<string>();
     List<string> onlyNotAfter = new List<string>();

     /// <summary>
     /// имя символа формата сиволчисло
     /// </summary>
     public string LetterName
     {
         get { return letterName; }
         set { letterName = value; }
     }
     
     /// <summary>
     /// Символы правила только не перед
     /// </summary>
     public List<string> OnlyNotBefore
     {
         get { return onlyNotBefore; }
         set { onlyNotBefore = value; }
     }

     /// <summary>
     /// символы правила только не после
     /// </summary>
     public List<string> OnlyNotAfter
     {
         get { return onlyNotAfter; }
         set { onlyNotAfter = value; }
     }

     /// <summary>
     /// символы правила только после
     /// </summary>
     public List<string> OnlyAfter
     {
         get { return onlyAfter; }
         set { onlyAfter = value; }
     }

     /// <summary>
     /// символы правила только перед
     /// </summary>
     public List<string> OnlyBefore
     {
         get { return onlyBefore; }
         set { onlyBefore = value; }
     }

     /// <summary>
     /// 
     /// </summary>
     /// <param name="name">имя символа</param>
     /// <param name="ldx">отступ слева</param>
     /// <param name="rdx">отступ справа</param>
     /// <param name="dy">отступ сверху</param>
     /// <param name="scale">масштаб</param>
     /// <param name="onlyafter"></param>
     /// <param name="onlybefore"></param>
     /// <param name="onlynotafter"></param>
     /// <param name="onlynotbefore"></param>
     public QHSRule(string name, double ldx, double rdx, double dy, double scale,
         string[] onlyafter, string[] onlybefore, string[] onlynotafter, string[] onlynotbefore)
     {
         letterName = name;
         leftDeltaX = ldx;
         rightDeltaX = rdx;
         deltaY = dy;
         this.scale = scale;
         onlyAfter = onlyafter.ToList();
         onlyBefore = onlybefore.ToList();
         onlyNotAfter = onlynotafter.ToList();
         onlyNotBefore = onlynotbefore.ToList();

     }

     /// <summary>
     /// 
     /// </summary>
     public double LeftDeltaX
     {
         get { return leftDeltaX; }
         set { leftDeltaX = value; }
     }

     /// <summary>
     /// Отступ справа
     /// </summary>
     public double RightDeltaX
     {
         get { return rightDeltaX; }
         set { rightDeltaX = value; }
     }

     /// <summary>
     /// Отступ сверху
     /// </summary>
     public double DeltaY
     {
         get { return deltaY; }
         set { deltaY = value; }
     }

     /// <summary>
     /// Масштаб буквы
     /// </summary>
     public double Scale
     {
         get { return scale; }
         set { scale = value; }
     }

     /// <summary>
     /// Проверка правила
     /// </summary>
     /// <param name="before"></param>
     /// <param name="after"></param>
     /// <returns></returns>
     public bool CheckRule(string before, string after)
     {
         // если что-то не так, то true
         if (before.Length == 0 && after.Length == 0) return true;
         // подготавливаем строчку
         if (!before.Contains('#')) before = "#" + before;
         // и вторую
         if (!after.Contains('#')) after = "#" + after;

         // если все гуд
         if (before.Length > 1)
         {
             // ну и линки
             if (onlyAfter.Count > 0)
                 return onlyAfter.Any(letter => letter == before);

             if (onlyNotAfter.Count > 0)
                 return onlyNotAfter.All(letter => letter != before);

         }
         // если все гуд
         if (after.Length > 1)
         {
             // линки линки
             if (onlyBefore.Count > 0)
                 return onlyBefore.Any(letter => letter == after);



             if (onlyNotBefore.Count > 0)
                 return onlyNotBefore.All(letter => letter != after);
         }
         return true;

     }

 }

/// <summary>
/// Управляющий класс, реализующий работу с QH скриптом
/// </summary>
public class QueenHandScript
{

    public static Dictionary<char, char> AssosDictionary = new Dictionary<char, char>(); 
    private List<QHSVariable> variables = new List<QHSVariable>();
    List<QHSRule> rules = new List<QHSRule>();
    private bool isExec = true;
    
	public QueenHandScript()
	{
		
	}

    /// <summary>
    ///  Главный метод, который подготавливает код к дальнейшей обработке.
    /// </summary>
    /// <param name="script"></param>

    public void BuildScript(string script)
    {
        
        // убрали все пробелы
        script = script.Trim();
        List<string> coms = new List<string>();

        // Реплейснули все переходы на новую строку, табуляции и др., все спецоператоры привели в божеский вид.
        script = script.Replace("\r", String.Empty).Replace("\n", String.Empty).Replace("only before", "onlybefore");
        script = script.Replace("only after", "onlyafter").Replace("only not before", "onlynotbefore");
        script = script.Replace("only not after", "onlynotafter").Replace("\t", String.Empty);

        // убрали все пробелы, длина которых больше 1
        script = System.Text.RegularExpressions.Regex.Replace(script, " +", " ");

        //список команд.
        List<string> args = script.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

        // очередь команд, которая передается в executecommands
           Queue<string> commands = new Queue<string>();


        // в этом цикле парсим по точке с запятой, если перед ней не стоят кавычки или решетка.
        for (int i = 0; i < args.Count; i++)
        {
            string str = args[i];
            if ((str[str.Length - 1] == '#' || str[str.Length - 1] == '\'') && i < args.Count - 1)
            {
                str += ";" + args[i + 1];
                i++;
            }

            commands.Enqueue(str);
        }

        // Переходим в управляющий метод
        ExecuteCommands(commands);
   
    }

    /// <summary>
    /// принимает команду, проверяет не является ли команда присвоением значения переменной.
    /// Если является, то изменяет значение переменной и возвращает false.
    /// Иначе возвращает true.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public bool SetValue(string command)
    {

        // Если только одно равно и при этом это не оператор сравнения
        if (command.Count(c => c == '=') == 1 && command.Count(c => c == '>') == 0 &&
            command.Count(c => c == '<') == 0 && command.Count(c => c == '!') == 0)
        {

            // парсим по пробелу.
            string[] args = command.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 0) return false;
            string name;
            
            // если это объявление переменной, то берем ее имя (1)
            if (args[0] == "var") name = args[1];
            // иначе берем (0)
            else name = args[0];
            
            

            //получаем значение, которое присваиваем.
            string ret = command.Substring(command.IndexOf("=", StringComparison.Ordinal)+1);
            ret = ret.Trim();

            //если это процедура, то делаем так.
            if (ret[0] == '$')
            {
                ret = ret.Substring(1);
                ret = ret.Replace("+", ";");
                AddVariable(name, ret);
            }
            else
            {
                // с помощью функции eval обрабатываем его.
                object obj = Eval(ret).ToString();
                // если обработалось - кидаем, что получили.
                if (obj.ToString() != "qhserror") AddVariable(name, obj.ToString());
                // иначе кидаем как строку.
                else AddVariable(name, ret.Trim());
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// заменяет имена переменных на их значения
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public string PrepareCommand(string command)
    {

        // если это не командаа присваивания
        if (!SetValue(command))
        {
            foreach (var variable in variables)
            {
                // если мы используем переменную как значение.
                if (command.Contains(" " + variable.Name + " "))
                    // заменяем имя на значение.
                    command = command.Replace(" " + variable.Name + " ", " " + variable.Value + " ");

                    // или если мы используем переменную как процедуру.
                else if (command == "$"+variable.Name) 
                    // заменяем на эту процедуру
                    BuildScript(variable.Value + ";");
            }
        }
        return command;
    }

    /// <summary>
    /// Главный обработчик скрипта
    /// </summary>
    /// <param name="innerCommands"></param>
    public void ExecuteCommands(Queue<string> innerCommands)
    {
        // получаем очередь и пока все не вытащим
        while (innerCommands.Count > 0)
        {
            // получаем строку
            string command = innerCommands.Dequeue();            
            // ее обрабатываем и подготавливаем
            command = PrepareCommand(command);
            // проверяем по опраторам 
            OperatorsExecute(command);
            // если мы не в ифе, либо ельзе, то 
            if (isExec)
            {
                // тут все обработчики. В дальнейшем будет библиотека
                CycleExecute(innerCommands, command);
                MethodsExecute(command);
                if (command == "hiddentest") Process.Start("notepad.exe");
            }
        }
    }

    /// <summary>
    /// обработчик операторов
    /// </summary>
    /// <param name="command"></param>
    public void OperatorsExecute(string command)
    {
        // сплитуем на слова
        string[] args = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (args.Length == 0) return;
        string ret;
        string ans;
        string numbers = "0123456789";

        switch (args[0])
        {   
                // если если :)
            case "if":
                int delta = 3;
                // существует также оператор если не. Тогда просто переворачиваем бул.
                if (args[1] == "not") delta += 4;
                // берем само выражение
                ret = command.Substring(command.IndexOf("if", StringComparison.Ordinal) + delta);
                // ивалуем его.
                ans = Eval(ret).ToString();
                // если ответ True или 1
                if (ans == "True" || ans == "1") isExec = true;
                // если ответ False или 0
                else if (ans == "False" || ans == "0") isExec = false;
                // если not
                if(delta != 3) isExec = !isExec;
                return;

                // если иначе
            case "else":
                // если наше инча также содержит иф, то проверяем, чтобы до этого у первого ифа условие не сошлось
                if (args.Length > 1 && args[1] == "if" && !isExec)
                {
                    // ну и дальше как в ифе
                     ret = command.Substring(command.IndexOf("if", StringComparison.Ordinal) + 3);
                     ans = Eval(ret).ToString();
                    if (ans == "True") isExec = true;
                    else if (ans == "False") isExec = false;
                }
                    // если же у нас просто else, переворачиваем
                else isExec = !isExec;
                return;
                // ну и если попали на endif, то значит вся лоика закончилась, просто ставим true;
            case "endif":
                isExec = true;
                return;
        }

        if (args.Length > 2 && args[0][0] == '#' && args[2][0] == '#' && args[0].Length == 3 
            && args[2].Length == 3 && numbers.Contains(args[0][2]) && numbers.Contains(args[2][2]))
        {
            switch (args[1])
            {
                case "onlyafter":
                    AddRule(args[0], new[] { args[2] }, new string[0], new string[0], new string[0]);       
                        break;

                case "onlybefore":
                        AddRule(args[0], new string[0], new[] { args[2] }, new string[0], new string[0]);
                        break;

                case "onlynotafter":
                        AddRule(args[0], new string[0], new string[0], new[] { args[2] }, new string[0]);
                        break;

                case "onlynotbefore":
                        AddRule(args[0], new string[0], new string[0],new string[0], new[] { args[2] });
                        break;
            }
        }
    }

    /// <summary>
    /// метод, возвращающий значение выражения
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public object Eval(string expression)
    {

        using (DataTable eval = new DataTable())
        {
            try
            {
                return eval.Compute(expression, null);
            }
            catch
            {
                return "qhserror";
            }
          
        }


        //try
        //{
        //    DataTable table = new DataTable();
        //    table.Columns.Add("expression", typeof (string), expression);
        //    DataRow row = table.NewRow();
        //    table.Rows.Add(row);
        //    return double.Parse((string) row["expression"]);
        //}
        //catch
        //{
        //    return -1;
        //}
    }

    /// <summary>
    /// Добавляет переменную в список переменных
    /// </summary>
    /// <param name="name">имя</param>
    /// <param name="value">значение</param>
    public void AddVariable(string name, string value)
    {
        // проверка на то, существует ли уже такая переменная
        foreach (QHSVariable variable in variables)
        {
            if (variable.Name == name)
            {
                // меняем ей значение
                variable.Value = value;
                return;
            }
        }
        // если нет, то добавляем
        variables.Add(new QHSVariable(name, value));
    }

    /// <summary>
    /// Удаляет переменную из списка
    /// </summary>
    /// <param name="name">имя</param>
    public void RemoveVariable(string name)
    {
        for (int i=0; i<variables.Count; i++)
        {
            if (variables[i].Name == name)
            {
                variables.Remove(variables[i]);
                return;
            }
        }
    }

    /// <summary>
    /// Обработчик циклов
    /// </summary>
    /// <param name="commands"></param>
    /// <param name="command"></param>
    public void CycleExecute(Queue<string> commands, string command)
    {
        try
        {
            // если это таки цикл
            if (command.Contains("from") && command.Contains("to"))
            {
                // сплитуем по словам
                string[] args = command.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                if (args.Length == 0) return;
                // если это нормальный полный цикл со всей инфой о себе и есть то, что его закрывает - endfrom
                if (args.Length > 7 && args[0] == "from" && args[2] == "to" && args[4] == "as" && args[6] == "by" &&
                    commands.Contains("endfrom"))
                {
                    // получаем все переменные из цикла
                    int from = Convert.ToInt32(args[1]);
                    int to = Convert.ToInt32(args[3]);
                    // задаем переменную цикла
                    AddVariable(args[5], args[1]);
                    int by = Convert.ToInt32(args[7]);
                    string str = commands.Dequeue();

                    // создаем свою очередь, это будет очередь команд, который будут выполняться в цикле.
                    List<string> innerCommands = new List<string>();
                    // и пока не конец цикла, загоняем все команды в эту очередь
                    while (str != "endfrom")
                    {
                        innerCommands.Add(str);
                        str = commands.Dequeue();
                    }

                    // Собственно, цикл
                    for (int i = from; i < to; i += by)
                    {
                        // Создаем очередь-буффер, чтоб не сожрала все команды из нашей
                        Queue<string> buffer = new Queue<string>();
                        // Загоняем все переменные 
                        foreach (string arg in innerCommands) buffer.Enqueue(arg);
                        // И экзекутим;
                        ExecuteCommands(buffer);
                        // меняем значение переменной цикла
                        AddVariable(args[5], i + "");
                    }
                    // при выходе удаляем переменную цикла
                    RemoveVariable(args[5]);

                }
            }
        }
        catch
        {
            
        }
    }

    /// <summary>
    /// Добавляет правило в список правил отображения почерка
    /// </summary>
    /// <param name="name">имя символа</param>
    /// <param name="ldx">отступ слева</param>
    /// <param name="rdx">отступ справа</param>
    /// <param name="dy">отступ сверху</param>
    /// <param name="scale">масштаб буквы</param>
    /// <param name="onlyafter"></param>
    /// <param name="onlybefore"></param>
    /// <param name="onlynotafter"></param>
    /// <param name="onlynotbefore"></param>
    public void AddRule(string name, 
        string[] onlyafter, string[] onlybefore, string[] onlynotafter, string[] onlynotbefore)
    {

        // Проверяем есть ли такая буква в правилах
        foreach (QHSRule rule in rules)
        {
            if (rule.LetterName == name)
            {
                // если есть - присваиваем и добавляем правила
                rule.OnlyAfter.AddRange(onlyafter);
                rule.OnlyBefore.AddRange(onlybefore);
                rule.OnlyNotAfter.AddRange(onlynotafter);
                rule.OnlyNotBefore.AddRange(onlynotbefore);
                return;
            }
        }
        // если нет - добавляем
        rules.Add(new QHSRule(name, 0, 0, 0, 1, onlyafter, onlybefore, onlynotafter, onlynotbefore));
    }

    /// <summary>
    /// Добавляет переменную правила с параметрами.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="ldx"></param>
    /// <param name="rdx"></param>
    /// <param name="dy"></param>
    /// <param name="scale"></param>
    public void AddRuleParams(string name, double ldx, double rdx, double dy, double scale)
    {

        // Проверяем есть ли такая буква в правилах
        foreach (QHSRule rule in rules)
        {
            if (rule.LetterName == name)
            {
                // если есть - присваиваем и добавляем правила
                rule.LeftDeltaX = ldx;
                rule.RightDeltaX = rdx;
                rule.DeltaY = dy;
                rule.Scale = scale;
                return;
            }
        }
        // если нет - добавляем
        rules.Add(new QHSRule(name, ldx, rdx, dy, scale, new string[0], new string[0],new string[0],new string[0]));
    }

    /// <summary>
    /// Возвращает правило для символа
    /// </summary>
    /// <param name="name">имя символа</param>
    /// <returns></returns>
    public QHSRule GetRule(string name)
    {
        if (!name.Contains('#')) name = "#" + name;
        return rules.FirstOrDefault(rule => rule.LetterName == name);
    }

    public void MethodsExecute(string command)
    {
       


            string firstc = command.Split(new[] {' '})[0];
            string substring = command.Substring(firstc.Length);

            if (firstc == "replace" && substring.Count(c => c == '\'') == 4)
            {
                substring = substring.Trim();
                string first = substring.Substring(substring.IndexOf("'") + 1);
                first = first.Remove(first.IndexOf("'"));
                substring = substring.Substring(first.Length + 2);

                substring = substring.Trim();
                string second = substring.Substring(substring.IndexOf("'") + 1);
                second = second.Remove(second.IndexOf("'"));
                AssosDictionary.Add(first[0], second[0]);

            }


            if (firstc == "set" && substring.Count(c => c == ',') >= 3)
            {
                substring = substring.Replace(" ", String.Empty).Trim();
                string[] ar = substring.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                List<string> args = new List<string>();
                for (int i = 0; i < ar.Length; i++)
                {
                    string buf = ar[i];
                    if (i < ar.Length - 1 && ar[i][ar[i].Length - 1] == '#')
                    {
                        buf += ar[i + 1];
                        i++;
                    }
                    args.Add(buf);
                }

                if (args.Count == 5 && args[0].Contains('#') && args[0].Length == 3 && "0123456789".Contains(args[0][2]) &&
                    "0123456789".Contains(args[1]) && "0123456789".Contains(args[2]) && "0123456789".Contains(args[3]) && "0123456789".Contains(args[4]))
                    AddRuleParams(args[0], Convert.ToInt32(args[1]), Convert.ToInt32(args[2]), Convert.ToInt32(args[3]), Convert.ToInt32(args[4]));
                
                
                    
                

            }
        
     
    }

}