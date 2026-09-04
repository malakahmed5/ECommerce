using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonResponses
{
    public class Result
    {
        private readonly List<Error> _errors = [];
        public bool IsSuccess => _errors.Count == 0;
        public bool IsFailure => !IsSuccess;
        public IReadOnlyList<Error> Errors => _errors;

        protected Result() { }
        protected Result(Error error)
        {
            _errors.Add(error);
        }
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }

        #region Factory Methods
        public static Result Ok() => new Result(); //Result.Ok() = new result()
        public static Result Fail(Error error) => new Result(error); //result.fail(error.NotFound())
        public static Result Fail(List<Error> errors) => new Result(errors);
        #endregion

        public static implicit operator Result(Error error) => Result.Fail(error);

    }

    public class Result<TValue>:Result
    {
        private TValue _value;
        public TValue Value => _value;

        private Result(TValue value):base()
        {
            _value = value; 
        }
        private Result(Error error):base(error)
        {
            _value = default!;
        }
        private Result(List<Error> errors):base(errors)
        {
            _value = default!;
        }

        #region Factory Methods
        public static Result<TValue> Ok(TValue value) => new Result<TValue>(value); //Result.Ok() = new result()
        public static new Result<TValue> Fail(Error error) => new Result<TValue>(error); //result.fail(error.NotFound())
        public static new Result<TValue> Fail(List<Error> errors) => new Result<TValue>(errors);
        #endregion

        public static implicit operator Result<TValue>(TValue value) => Result<TValue>.Ok(value);
        public static implicit operator Result<TValue>(Error error) => Result<TValue>.Fail(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Result<TValue>.Fail(errors);  
    }

}
