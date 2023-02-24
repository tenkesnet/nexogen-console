﻿using NexogenLogger;

Logger logger = new Logger();
try
{
    logger = new Logger("");
} catch(Exception e)
{
    logger?.error(e.Message);
}
try
{
    await logger.info(" If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.");
    await logger.debug("Test Value: 23");
    await logger.error("Fatal error");
}
catch (LongLogMessageException e)
{
    logger?.error(e.Message);
}

Console.WriteLine("Please press the <Entery>.")
Console.ReadLine();
