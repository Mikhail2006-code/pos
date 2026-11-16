using Microsoft.Extensions.DependencyInjection;
using Pos.ConsoleApp;

using var provider = CompositionRoot.Build();

var menu = provider.GetRequiredService<ConsoleMenu>();
menu.Run();