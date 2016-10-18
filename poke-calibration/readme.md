# Poke Calibration

1. Open the poke calibration workflow (poke-calibration.bonsai).

# Poke Testing

1. Open the poke testing workflow (poke-test.bonsai).
2. Set the PortName to the poke you want to test and run the workflow.
3. Test LED on/off.
4. Test Piezo buzzer (availability tone and reward tone).
5. Test poke sensor (physically trigger the poke).
6. Test valve (toggle water valve on/off - you should HEAR it clicking).

## Arduino Parameters

Model: Arduino Uno
Left Poke Port: COM6 @ 57600bps
Right Poke Port: COM4 @ 57600bps

| Wire               | Pin |
| ------------------ | ----|
| Poke LED           | 5   |
| Poke sensor        | 6   |
| Valve              | 2   |
| Piezo buzzer       | 3   |
