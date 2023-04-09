namespace CaNewralNet;

using CryptoExchange.Net.CommonObjects;
using static Tensorflow.Binding;
using static Tensorflow.KerasApi;
using static Tensorflow.tensorflow;
using Tensorflow;
using Tensorflow.Keras;
using Tensorflow.ModelSaving;
using Tensorflow.Keras.ArgsDefinition;
using Tensorflow.Keras.Engine;
using Tensorflow.Keras.Layers;
using Tensorflow.Keras.Metrics;
using Tensorflow.NumPy;
using Tensorflow.Keras.Layers.Rnn;
using Tensorflow.Keras.Optimizers;
using Tensorflow.Keras.Losses;

public class CaPredictor
{
    private List<Kline> _data;
    Sequential model;
    public CaPredictor(List<Kline> data)
    {
        _data = data;

        SequentialArgs args = new SequentialArgs();
        args.Trainable = true;
        args.Layers.Add(new Reshape(new Shape(-1, 50, 5), InputSpec: new Shape(250)));
        args.Layers.Add(new LSTN(64));
            //new[] {
            //    new Reshape(new Shape(-1, 50, 5), inputShape: new Shape(250)),
            //    new LSTM(64),
            //    new Dense(32, activation: ActivationType.Relu),
            //    new Dense(5)
            //};
        // Define model architecture
        model = new Sequential(args);
    }

    public void TrainModel()
    {
        // Compile model with optimizer, loss function, and metrics
        model.compile(optimizer: new Adam(),
                      loss: new MeanSquaredError(),
                      metrics: new List<ILossOrMetric>() { new MeanAbsoluteError() });

        // Prepare data for training
        var input = new List<double>();
        var target = new List<double>();

        for (int i = 0; i < _data.Count - 55; i++)
        {
            for (int j = i; j < i + 50; j++)
            {
                input.Add((double)_data[j].OpenPrice!);
                input.Add((double)_data[j].HighPrice!);
                input.Add((double)_data[j].LowPrice!);
                input.Add((double)_data[j].ClosePrice!);
                input.Add((double)_data[j].Volume!);
            }

            target.Add((double)_data[i + 55].ClosePrice!);
        }

        // Convert data to tensors
        var x = new DenseTensor<double>(input.ToArray(), new long[] { input.Count / 250, 50, 5 });
        var y = new DenseTensor<double>(target.ToArray(), new long[] { target.Count, 1 });

        // Train model
        model.fit(x, y, epochs: 50, batch_size: 64, validation_split: 0.2f);
    }

    public List<double> PredictNextClosePrice()
    {
        // Prepare input data for prediction
        var input = new List<double>();

        for (int i = _data.Count - 50; i < _data.Count; i++)
        {
            input.Add((double)_data[i].OpenPrice!);
            input.Add((double)_data[i].HighPrice!);
            input.Add((double)_data[i].LowPrice!);
            input.Add((double)_data[i].ClosePrice!);
            input.Add((double)_data[i].Volume!);
        }

        // Convert data to tensor
        var x = new DenseTensor<double>(input.ToArray(), new long[] { 1, 50, 5 });

        // Make prediction
        var prediction = model.predict(x);

        // Convert prediction to list of doubles and return
        var result = new List<double>();

        foreach (double val in prediction.GetData<double>())
        {
            result.Add(val);
        }

        return result;
    }
}
