Основная работа приложения:

1) Приложение принимает в качестве аргумента ссылку (входящая ссылка);

2) Проходится по всем ссылкам на HTML странице, соответсвующей входящей ссылки и проверяет Status Code ответа по этим ссылкам.

3) Вынести в файл конфигурации параметр, отвечающий за степень вложенности. Например, если степень вложенности 2, то приложение проходит по входящей ссылке и по всем ссылкам на соответствующей ей HTML странице.

4) На основании этих данных сформировать .csv отчет о состоянии ссылок, название и путь до которого конфигурируется в файле конфигурации. В отчете будут две колонки: URL, Status Code.

Пример отчета:

URL,Status Code

http://en.wikipedia.org/wiki/Bavaria,200

5) Возникающие исключения в работе приложения должны отправляться на конфигурируемый почтовый ящик.

6) Обрабатываемые ссылки могут быть как относительные, так и абсолютные. Парсинг ссылок осуществляется с помощью Regex.

7) Информация об обработанных ссылках должна добавляться в базу данных SQL при помощи технологии EntityFramework;

8) В базу данных должна храниться следующая информация (помимо самой информации о ссылках и статус кодах): тип ссылки (абсолютная или относительная), кто запустил проверку (логин), время начала обработки, время окончания, количество обработанных ссылок;

9) После окончания обработки должен формироваться отчет в формате html, разметку которого можно конфигурировать, в котором все ссылки сгруппированы по статус кодам с нормальными заголовками (200 – Успешно обработанные ссылки, 404 – Объекты не найдены и т.д.);

10) Отчет должен быть отправлен на конфигурируемый почтовый адрес;

11) Данные для отчета должны быть взяты из БД используя технологию ADO.NET
