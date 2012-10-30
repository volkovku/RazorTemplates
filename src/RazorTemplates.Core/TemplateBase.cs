using System.Text;

namespace RazorTemplates.Core
{
    public abstract class TemplateBase
    {
        private readonly StringBuilder _buffer = new StringBuilder();

        public dynamic Model
        {
            get;
            set;
        }

        #region Execute

        public virtual string Render(object model)
        {
            Model = model;

            Execute();

            return _buffer.ToString();
        }

        public abstract void Execute();

        #endregion

        #region Write

        protected void Write(char value)
        {
            _buffer.Append(value);
        }

        protected void Write(char[] value)
        {
            _buffer.Append(value);
        }

        protected void Write(string value)
        {
            _buffer.Append(value);
        }

        protected void Write(bool value)
        {
            _buffer.Append(value);
        }

        protected void Write(byte value)
        {
            _buffer.Append(value);
        }

        protected void Write(int value)
        {
            _buffer.Append(value);
        }

        protected void Write(long value)
        {
            _buffer.Append(value);
        }

        protected void Write(double value)
        {
            _buffer.Append(value);
        }

        protected void Write(float value)
        {
            _buffer.Append(value);
        }

        protected void Write(decimal value)
        {
            _buffer.Append(value);
        }

        protected void Write(object value)
        {
            _buffer.Append(value);
        }

        #endregion

        #region WriteLiteral

        protected void WriteLiteral(char value)
        {
            _buffer.Append(value);
        }

        protected void WriteLiteral(char[] value)
        {
            _buffer.Append(value);
        }

        protected void WriteLiteral(byte value)
        {
            _buffer.Append(value);
        }

        protected void WriteLiteral(int value)
        {
            _buffer.Append(value);
        }

        protected void WriteLiteral(long value)
        {
            _buffer.Append(value);
        }

        protected void WriteLiteral(double value)
        {
            _buffer.Append(value);
        }

        protected void WriteLiteral(float value)
        {
            _buffer.Append(value);
        }

        protected void WriteLiteral(decimal value)
        {
            _buffer.Append(value);
        }

        protected void WriteLiteral(object value)
        {
            _buffer.Append(value);
        }

        #endregion
    }
}