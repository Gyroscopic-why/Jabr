using System;

using static System.Console;


using static Jabr.GlobalSettings;
using static Jabr.GlobalVariables;



namespace Jabr
{
    internal class ProgramInfo
    {
        static public void ShowProgramInfo()
        {
            Write("\n\n\n");
            if (gClearUsed)
            {
                Clear();
                ForegroundColor = ConsoleColor.Gray;
                Write("\n\n\n\t\t\t   Добро пожаловать в Jabr " + gProgramVersion + "!\n\n\n");
                Write("\t\t\t--->  Информация о программе  <---\n\n");
            }

            ForegroundColor = ConsoleColor.White;
            Write("\t\t[i]  - Большое спасибо что всё ещё пользуетесь моим шифратором!\n\n");
            ForegroundColor = ConsoleColor.Gray;

            Write("\t\t[!]  - Спешу сообщить, что текущая версия клиента ");
            ForegroundColor = ConsoleColor.DarkGray;
            Write("(");
            ForegroundColor = ConsoleColor.Cyan;
            Write(gProgramVersion);
            ForegroundColor = ConsoleColor.DarkGray;
            Write(")\n");
            Write("\t\t        (а также все остальные ");
            ForegroundColor = ConsoleColor.Cyan;
            Write("интерфейсы 4 поколения");
            ForegroundColor = ConsoleColor.DarkGray;
            Write(" и ниже)\n");

            ForegroundColor = ConsoleColor.Gray;
            Write("\t\t       являются ");
            ForegroundColor = ConsoleColor.Red;
            Write("устаревшими");
            ForegroundColor = ConsoleColor.Gray;
            Write(" и больше ");
            ForegroundColor = ConsoleColor.Red;
            Write("не будет получать обновлений :(\n\n");
            ForegroundColor = ConsoleColor.Gray;

            Write("\t\t       Это означает, что в будущем могут возникнуть ");
            ForegroundColor = ConsoleColor.Red;
            Write("повторные проблемы");
            ForegroundColor = ConsoleColor.Gray;
            Write(" с обратной ");
            ForegroundColor = ConsoleColor.Red;
            Write("несовместимостью\n");
            ForegroundColor = ConsoleColor.Gray;
            Write("\t\t       которые я ");
            ForegroundColor = ConsoleColor.Red;
            Write("не обещаю исправить ");
            ForegroundColor = ConsoleColor.DarkGray;
            Write("(прямо как с версиями ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("v1.4.2");
            ForegroundColor = ConsoleColor.DarkGray;
            Write(" и ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("v1.4.3");
            ForegroundColor = ConsoleColor.DarkGray;
            Write(" [текущей])\n\n");
            ForegroundColor = ConsoleColor.Gray;

            Write("\t\t[↑]  - Также это значит что ");
            ForegroundColor = ConsoleColor.Green;
            Write("уже вышла новая кросс платформенная версия клиента ");
            ForegroundColor = ConsoleColor.Cyan;
            Write("5 поколения\n");
            ForegroundColor = ConsoleColor.DarkGray;
            Write("\t\t       (под операционные системы ");
            ForegroundColor = ConsoleColor.Cyan;
            Write("Windows, Linux, MacOS");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("[beta] ");
            ForegroundColor = ConsoleColor.Cyan;
            Write("и Android");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("[beta]");
            ForegroundColor = ConsoleColor.DarkGray;
            Write(")\n");
            ForegroundColor = ConsoleColor.Gray;
            Write("\t\t       с новым ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("протоколом шифрования");
            ForegroundColor = ConsoleColor.Gray;
            Write(", автоматической ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("генерацией ключей");
            ForegroundColor = ConsoleColor.Gray;
            Write(" и другими ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("нововведениями");
            ForegroundColor = ConsoleColor.Green;
            Write(" :)\n\n");
            ForegroundColor = ConsoleColor.Gray;

            Write("\t\t[!]  - Данное решение было принято по причине ");
            ForegroundColor = ConsoleColor.Red;
            Write("не-кроссплатформенности");
            ForegroundColor = ConsoleColor.Cyan;
            Write(" клиентов текущего поколения 4,\n");
            ForegroundColor = ConsoleColor.Red;
            Write("\t\t       невозможности внедрить ");
            ForegroundColor = ConsoleColor.Gray;
            Write("в старую систему новые ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("протоколы шифрования");
            ForegroundColor = ConsoleColor.Gray;
            Write(" и ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("другие новые системы,\n");
            ForegroundColor = ConsoleColor.Gray;
            Write("\t\t       без переделывания всей архитектуры программы с нуля\n");
            ForegroundColor = ConsoleColor.DarkGray;
            Write("\t\t       (новый протокол шифрования ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("РЕ5");
            ForegroundColor = ConsoleColor.Green;
            Write(" уже разработан ");
            ForegroundColor = ConsoleColor.DarkGray;
            Write("на момент выхода данной завершающей версии!)\n\n\n");


            ForegroundColor = ConsoleColor.White;
            Write("\t\t[=]  - Теперь про создание последней легаси версии ");
            ForegroundColor = ConsoleColor.Cyan;
            Write(gProgramVersion);
            ForegroundColor = ConsoleColor.DarkGray;
            Write(" (текущий клиент)\n\n");
            ForegroundColor = ConsoleColor.Gray;

            Write("\t\t       Это последнее обновление в старом ");
            ForegroundColor = ConsoleColor.Cyan;
            Write("поколении 4");
            ForegroundColor = ConsoleColor.Gray;
            Write(" но при этом текущая версия\n");
            ForegroundColor = ConsoleColor.Red;
            Write("\t\t        > НЕ совместима");
            ForegroundColor = ConsoleColor.Cyan;
            Write(" с поколением 4 ");
            ForegroundColor = ConsoleColor.Gray;
            Write("из-за изменений логики протоколов ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("РЕ3");
            ForegroundColor = ConsoleColor.Gray;
            Write(" и ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("РЕ4\n");
            ForegroundColor = ConsoleColor.Green;
            Write("\t\t        > Совместима ");
            ForegroundColor = ConsoleColor.DarkGray;
            Write("(протоколы ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("РЕ3");
            ForegroundColor = ConsoleColor.DarkGray;
            Write(" и ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("РЕ4");
            ForegroundColor = ConsoleColor.DarkGray;
            Write(")");
            ForegroundColor = ConsoleColor.Gray;
            Write(" некоторое время с клиентами ");
            ForegroundColor = ConsoleColor.Cyan;
            Write("поколения 5\n\n");
            ForegroundColor = ConsoleColor.Gray;

            Write("\t\t[i]  - Причины по которой я доделываю это последнее обновление:\n");
            ForegroundColor = ConsoleColor.Green;
            Write("\t\t          - обновление безопасности ");
            ForegroundColor = ConsoleColor.Gray;
            Write("для пользователей протоколов ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("РЕ3");
            ForegroundColor = ConsoleColor.Gray;
            Write(" и ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("РЕ4\n");
            ForegroundColor = ConsoleColor.Green;
            Write("\t\t          - восстановление совместимости");
            ForegroundColor = ConsoleColor.Gray;
            Write(" на некоторое время с ");
            ForegroundColor = ConsoleColor.Cyan;
            Write("5 поколением\n");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("\t\t          - обеспечение");
            ForegroundColor = ConsoleColor.Gray;
            Write(" дополнительного ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("времени на переход");
            ForegroundColor = ConsoleColor.Gray;
            Write(" для пользователей ");
            ForegroundColor = ConsoleColor.Cyan;
            Write("старого клиента\n");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("\t\t          - продление жизни ");
            ForegroundColor = ConsoleColor.Cyan;
            Write("4 поколения клиента");
            ForegroundColor = ConsoleColor.Gray;
            Write(" для замедления скорости устаревания\n");
            ForegroundColor = ConsoleColor.DarkGray;
            Write("\t\t             (хотя бы не сразу после выхода ");
            ForegroundColor = ConsoleColor.Cyan;
            Write("поколения 5");
            ForegroundColor = ConsoleColor.DarkGray;
            Write(")\n\n");
            ForegroundColor = ConsoleColor.Gray;

            Write("\t\t       Это произошло из-за пересмотра старых протоколов, что привело к");
            ForegroundColor = ConsoleColor.Green;
            Write(" улучшению их безопасности\n");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("\t\t       путём изменения логики работы");
            ForegroundColor = ConsoleColor.DarkGray;
            Write(" - что ");
            ForegroundColor = ConsoleColor.Red;
            Write("ломает обратную совместимость со ");
            ForegroundColor = ConsoleColor.Cyan;
            Write("старыми версиями\n\n");
            ForegroundColor = ConsoleColor.Gray;

            Write("\t\t       Данное решение было ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("трудным, но необходимым");
            ForegroundColor = ConsoleColor.Gray;
            Write(" для поддержания ");
            ForegroundColor = ConsoleColor.Green;
            Write("уровня безопасности,\n");
            ForegroundColor = ConsoleColor.DarkGray;
            Write("\t\t       особенно в таких непростых временах как сейчас\n\n");
            ForegroundColor = ConsoleColor.Gray;

            Write("\t\t[↑]  - Настоятельно рекомендую ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("перейти на новый клиент");
            ForegroundColor = ConsoleColor.Cyan;
            Write(" 5 поколения ");
            ForegroundColor = ConsoleColor.DarkGray;
            Write("(или выше) ");
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("для:\n");
            ForegroundColor = ConsoleColor.Green;
            Write("\t\t         - минимизации риска");
            ForegroundColor = ConsoleColor.Gray;
            Write(" повторной ");
            ForegroundColor = ConsoleColor.Red;
            Write("обратной несовместимости\n");
            ForegroundColor = ConsoleColor.Green;
            Write("\t\t         - отсутствие угроз безопасности,");
            ForegroundColor = ConsoleColor.Gray;
            Write(" в случае ");
            ForegroundColor = ConsoleColor.Red;
            Write("обнаружения уязвимости\n");
            ForegroundColor = ConsoleColor.Gray;
            Write("\t\t           которая");
            ForegroundColor = ConsoleColor.Red;
            Write(" НЕ будет исправлена ");
            ForegroundColor = ConsoleColor.Cyan;
            Write("в старых версиях\n\n");
            ForegroundColor = ConsoleColor.White;

            Write("\t\t[♥]  - Спасибо за понимание данной ситуации.\n");
            Write("\t\t       Всего наилучшего, Gyroscopic\n\n");


            ForegroundColor = ConsoleColor.DarkGray;
            Write("\t\t       (нажмите любую клавишу для возврата в меню)");
            ForegroundColor = ConsoleColor.Black;
            ReadKey();

            Clear();
            ForegroundColor = ConsoleColor.Gray;
            Write("\n\n\n\t\t\t   Добро пожаловать в Jabr " + gProgramVersion + "!");
        }
    }
}
