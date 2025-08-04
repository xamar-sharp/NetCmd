#NetCmd

## Описание
Консольное приложение на C# для централизованного доступа к функциональности и комплексным операциям экосистемы .NET5.
Данное решение спроектировано с учетом рекомендаций SOLID-принципов для расширяемости и распределенности внутренней архитектуры приложения.
## Установка
Выполните эту команду в командной строке Windows:
```bash
git clone https://github.com/xamar-sharp/NetCmd.git
```
## Использование
	Данное приложение опубликовано в development-форме и может быть вами модифицировано,
при наличии установленной на вашем устройстве платформы `.NET5`.
Для создания собственной внутренней команды в приложении выполните нижеуказанные действия.
1.	Реализуйте интерфейс Infrastructure/IEntryPackage:
	```C#
	internal interface IEntryPackage
{
    IEntryPackage WithEntry(IEntry entry);
    Task<IList<IEntry>> ProvideEntriesAsync();
}
	```
	Примером реализации пакета для кастомных команд явлется пакет команд по умолчанию Defaults/NeToolsPackage:
	```C#
	internal class NeToolsPackage : IEntryPackage
{
    private readonly IList<IEntry> _networkTools;
    public NeToolsPackage()
    {
        _networkTools = new List<IEntry>(4);
        this.WithEntry(new DnsEntry()).WithEntry(new InstallEntry()).WithEntry(new HelpEntry()).
            WithEntry(new HttpEntry()).WithEntry(new StateEntry()).WithEntry(new LoaderEntry()).
            WithEntry(new ProcessStartEntry()).WithEntry(new DeadEntry()).WithEntry(new ReaderEntry()).
            WithEntry(new DirectoryIteratorEntry()).WithEntry(new DriveEntry()).WithEntry(new SpeechEntry()).
            WithEntry(new ConsoleEntry()).WithEntry(new AscIIDrawEntry()).WithEntry(new FunctionEntry());
    }
    public IEntryPackage WithEntry(IEntry entry)
    {
        _networkTools.Add(entry);
        return this;
    }
    public async Task<IList<IEntry>> ProvideEntriesAsync()
    {
        await Task.Yield();
        return _networkTools;
    }
}
	```
2.		Каждая из команд в пакете команд IEntryPackage представляет из себя класс,
	реализуюший интерфейс Infrastructure/IEntry:
	```C#
	internal interface IEntry : IUIMessager
{
    public int ParameterCount { get; }
    public string CommandName { get; }
    public string HelpText { get; }
    public ConsoleColor HelpColor { get; }
    public void React(string[] paramsRaw);
}
	```
	Реализуйте таким образом свои команды и добавьте их в коллекцию команд в конструкторе IEntryPackage.
3.		Теперь добавьте свой пакет команд новым вызовом AddPackage в классе Program, опираясь на 
	существующий шаблон по умолчанию:
	```C#
	internal class Program
{
    static async Task Main(string[] args)
    {
        (await new EntryBuilder().AddPackage(new NeToolsPackage())).Build().Run();
    }
}
	```
## Лицензия 
MIT