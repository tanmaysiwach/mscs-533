import 'package:flutter/material.dart';

void main() => runApp(MeasureConverterApp());

class MeasureConverterApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Measures Converter',
      theme: ThemeData(primarySwatch: Colors.blue),
      home: MeasureConverterScreen(),
    );
  }
}

class MeasureConverterScreen extends StatefulWidget {
  @override
  _MeasureConverterScreenState createState() => _MeasureConverterScreenState();
}

class _MeasureConverterScreenState extends State<MeasureConverterScreen> {
  final TextEditingController _controller = TextEditingController();
  String _fromUnit = 'meters';
  String _toUnit = 'feet';
  String _result = '';

  final Map<String, double> conversionRatesToMeters = {
    'meters': 1.0,
    'feet': 0.3048,
    'inches': 0.0254,
    'kilometers': 1000.0,
    'miles': 1609.34,
  };

  double _convert(String from, String to, double value) {
    double valueInMeters = value * conversionRatesToMeters[from]!;
    return valueInMeters / conversionRatesToMeters[to]!;
  }

  void _onConvert() {
    double? inputValue = double.tryParse(_controller.text);
    if (inputValue == null) {
      setState(() {
        _result = 'Invalid input';
      });
      return;
    }

    double converted = _convert(_fromUnit, _toUnit, inputValue);
    setState(() {
      _result = '$inputValue $_fromUnit are ${converted.toStringAsFixed(3)} $_toUnit';
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Measures Converter')),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          children: [
            Text('Value', style: TextStyle(fontSize: 20)),
            TextField(
              controller: _controller,
              keyboardType: TextInputType.number,
              decoration: InputDecoration(hintText: 'Enter value'),
            ),
            SizedBox(height: 20),
            Text('From', style: TextStyle(fontSize: 18)),
            DropdownButton<String>(
              value: _fromUnit,
              onChanged: (value) => setState(() => _fromUnit = value!),
              items: conversionRatesToMeters.keys
                  .map((unit) => DropdownMenuItem(value: unit, child: Text(unit)))
                  .toList(),
            ),
            SizedBox(height: 10),
            Text('To', style: TextStyle(fontSize: 18)),
            DropdownButton<String>(
              value: _toUnit,
              onChanged: (value) => setState(() => _toUnit = value!),
              items: conversionRatesToMeters.keys
                  .map((unit) => DropdownMenuItem(value: unit, child: Text(unit)))
                  .toList(),
            ),
            SizedBox(height: 20),
            ElevatedButton(
              onPressed: _onConvert,
              child: Text('Convert'),
            ),
            SizedBox(height: 20),
            Text(
              _result,
              style: TextStyle(fontSize: 18),
            ),
          ],
        ),
      ),
    );
  }
}
