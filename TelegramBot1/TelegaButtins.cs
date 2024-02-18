using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot1;

public partial class Telega
{
    InlineKeyboardMarkup GetInlineCoinBtns()
    {
        List<string> btns = Db.GetCurBandles();
        List<InlineKeyboardButton> inlineBtnRow = new();
        List<List<InlineKeyboardButton>> inlineBtns = new();
        
        foreach (string btn in btns)
        {
            if ((inlineBtnRow.Count + 1) % 5 == 0)
            {
                List<InlineKeyboardButton> r = new();
                foreach (var b in inlineBtnRow) r.Add(b);
                
                inlineBtns.Add(r);
                inlineBtnRow = new();
            }
            var kb = new InlineKeyboardButton(btn);
            kb.CallbackData = btn;
            inlineBtnRow.Add(kb);
        }

        if(inlineBtnRow.Count < 4)
            inlineBtns.Add(inlineBtnRow);

        InlineKeyboardMarkup inlineCoinBtns = new(inlineBtns);
        return inlineCoinBtns;
    }

    ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(
        new KeyboardButton[][]
        {
            new KeyboardButton[] { "d 1.1", "d 1.2" },
            new KeyboardButton[] { "d 2.1", "d 2.2" },
        }
    );
}
