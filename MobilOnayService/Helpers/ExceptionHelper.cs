using System;

namespace MobilOnayService.Helpers
{
    public static class ExceptionHelper
    {
        public static Exception Throw(Exception exception, string className, string methodName)
        {
            var message = exception.Message;
            if (message[^1] != ']')
                message += Environment.NewLine + Environment.NewLine + "trace --->";

            return new Exception(message + Environment.NewLine + "[" + className + "." + methodName + "]", exception.InnerException);
        }

        public static string ReplaceLineBreak(Exception ex)
        {
            return ex.Message.Replace(Environment.NewLine, "<br>");
        }
    }
}
