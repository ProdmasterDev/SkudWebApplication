//window.serialPortHandler = new SerialPortHandler(
//    { baudRate: 9600 });

//// Считывание карты

//window.readCard = async () => {
//    if (!window.serialPortHandler.isOpened) {
//        await window.serialPortHandler.open();
//    }
//    return await window.serialPortHandler.read()
//}