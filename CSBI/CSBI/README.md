# Задание

Написать сервис REST Api.
Добавить JWT-авторизацию(регистрация + авторизация). Как хранить
учетные данные значения не имеет, хоть в файле.

Все методы(кроме регистрации и авторизации) должны требовать
авторизацию.

Создать метод который возвращает время, когда авторизованный
пользователь проводил успешные авторизации(список), с какого Ip.

Создать метод который возвращает время, когда заданный(по id)
пользователь проводил успешные авторизацию(список), с какого Ip.

На основе Net Core Midllware создать альтернативную JWT-
авторизации логику: если в заголовках запроса есть параметр
secure-admin со значением e98d2243-8fd5-40f6-990f-48ca312b8aa5 -
пропускать обычную авторизацию и устанавливать вручную Id
пользователя на указанный Guid.

Весь написанный код должен по мере обоснованности
руководствоваться принципам SOLID.

Требуется покрытие unit-тестами написанного функционала.
