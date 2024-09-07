using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace neleo_com.Logic.Bridges.Velux {

    /// <summary>
    ///   Helper methods to access the buffer (Byte Array) in a datagram.</summary>
    public static class Klf200DatagramHelper {

        /// <summary>
        ///   Definition of the start date/time of the Unix epoch.</summary>
        private static readonly DateTime UtcMinUnixDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        ///   Verifies if the specified byte frame is accessible in <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <param name="length">
        ///   Length od the data chunk.</param>
        /// <returns>
        ///   <c>true</c> if the data chunk is accessible in <paramref name="buffer"/>; otherwise <c>false</c>.</returns>
        public static void CheckDataBoundaries(this Byte[] buffer, Int32 pos, Int32 length) {

            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            if (pos < 0)
                throw new ArgumentOutOfRangeException(nameof(pos));

            if (length < 1)
                throw new ArgumentOutOfRangeException(nameof(length));

            if (pos + length > buffer.Length)
                throw new ArgumentOutOfRangeException(nameof(buffer));

        }

        /// <summary>
        ///   Reads a <see cref="Byte"/> from <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <returns>
        ///   The value.</returns>
        public static Byte ReadByte(this Byte[] buffer, Int32 pos) {

            // verify boundaries
            buffer.CheckDataBoundaries(pos, 1);

            // return value
            return buffer[pos];

        }

        /// <summary>
        ///   Reads a <see cref="Byte"/> Array from <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <param name="length">
        ///   Length of the data chunk.</param>
        /// <returns>
        ///   The value.</returns>
        public static Byte[] ReadBytes(this Byte[] buffer, Int32 pos, Int32 length) {

            // verify boundaries
            buffer.CheckDataBoundaries(pos, length);

            // return value
            return buffer.Skip(pos).Take(length).ToArray();

        }

        /// <summary>
        ///   Reads a <see cref="Byte"/> Array from <paramref name="buffer"/> in reverse order 
        ///   to convert multi-byte arrays between little-endian and big-endian format.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <param name="length">
        ///   Length of the data chunk.</param>
        /// <returns>
        ///   The value.</returns>
        public static Byte[] ReadBytesReverse(this Byte[] buffer, Int32 pos, Int32 length) {

            // verify boundaries
            buffer.CheckDataBoundaries(pos, length);

            // return value
            return buffer.Skip(pos).Take(length).Reverse().ToArray();

        }

        /// <summary>
        ///   Reads a <see cref="UInt16"/> (2 Bytes with big-endian encoding) from <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <returns>
        ///   The value.</returns>
        public static UInt16 ReadUInt16(this Byte[] buffer, Int32 pos) {

            // return value (boundary validation happens in ReadBytes())
            return BitConverter.ToUInt16(buffer.ReadBytesReverse(pos, 2), 0);

        }

        /// <summary>
        ///   Reads a <see cref="UInt32"/> (4 Bytes with big-endian encoding) from <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <returns>
        ///   The value.</returns>
        public static UInt32 ReadUInt32(this Byte[] buffer, Int32 pos) {

            // return value (boundary validation happens in ReadBytes())
            return BitConverter.ToUInt32(buffer.ReadBytesReverse(pos, 4), 0);

        }

        /// <summary>
        ///   Reads a <see cref="UInt64"/> (8 Bytes with big-endian encoding) from <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <returns>
        ///   The value.</returns>
        public static UInt64 ReadUInt64(this Byte[] buffer, Int32 pos) {

            // return value (boundary validation happens in ReadBytes())
            return BitConverter.ToUInt64(buffer.ReadBytesReverse(pos, 8), 0);

        }

        /// <summary>
        ///   Reads a <see cref="String"/> from <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <param name="length">
        ///   Length of the data chunk.</param>
        /// <returns>
        ///   The value.</returns>
        public static String ReadString(this Byte[] buffer, Int32 pos, Int32 length) {

            // return value (boundary validation happens in ReadBytes())
            // ensure to trim empty (0 byte) characters first
            return Encoding.UTF8.GetString(buffer.ReadBytes(pos, length)).Trim(new Char[] { '\0' });

        }

        /// <summary>
        ///   Reads an Enumeration (1, 2, 4 or 8 byte numbers only) from <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <param name="length">
        ///   Length of the data chunk.</param>
        /// <param name="fallback">
        ///   Default value will be used if mapping isn't possible or enum type doesn't match data chunk length.</param>
        /// <returns>
        ///   The (mapped or fallback) value.</returns>
        public static T ReadEnum<T>(this Byte[] buffer, Int32 pos, Int32 length, T fallback) where T : Enum {

            // read value based on underlaying enum type (boundary validation happens in Read<Type>())
            Object data = null;
            switch (fallback.GetTypeCode()) {

                case TypeCode.Byte:
                    if (length == 1)
                        data = buffer.ReadByte(pos);
                    break;

                case TypeCode.UInt16:
                    if (length == 2)
                        data = buffer.ReadUInt16(pos);
                    break;

                case TypeCode.UInt32:
                    if (length == 4)
                        data = buffer.ReadUInt32(pos);
                    break;

                case TypeCode.UInt64:
                    if (length == 8)
                        data = buffer.ReadUInt64(pos);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(T));

            }

            // map and return value (or fallback value, if mapping isn't successful)
            Type type = typeof(T);
            if (data != null)
                return Enum.IsDefined(type, data) ? (T)Enum.ToObject(type, data) : fallback;
            else
                return fallback;

        }

        /// <summary>
        ///   Reads the date and time from <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <param name="length">
        ///   Length of the data chunk.</param>
        /// <returns>
        ///   The value (.net DateTime).</returns>
        public static DateTime ReadDateTime(this Byte[] buffer, Int32 pos) {

            // read and convert value (boundary validation happens in ReadUInt32())
            return Klf200DatagramHelper.UtcMinUnixDateTime.AddSeconds(buffer.ReadUInt32(pos));

        }

        /// <summary>
        ///   Reads the data into a bit array (little-endian).</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <param name="length">
        ///   Length of the data chunk.</param>
        /// <returns>
        ///   The value.</returns>
        public static BitArray ReadBits(this Byte[] buffer, Int32 pos, Int32 length) {

            // return value (boundary validation happens in ReadBytes())
            return new BitArray(buffer.ReadBytes(pos, length));

        }

        /// <summary>
        ///   Sets all Bytes in a data chunk to zero.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <param name="length">
        ///   Length of the data chunk.</param>
        public static void WipeData(this Byte[] buffer, Int32 pos, Int32 length) {

            // verify boundaries
            buffer.CheckDataBoundaries(pos, length);

            // wipe data
            Buffer.BlockCopy(new Byte[length], 0, buffer, pos, length);

        }

        /// <summary>
        ///   Writes a Byte to <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="value">
        ///   The value.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        public static void WriteByte(this Byte[] buffer, Byte value, Int32 pos) {

            // verify boundaries
            buffer.CheckDataBoundaries(pos, 1);

            // set value
            buffer[pos] = value;

        }

        /// <summary>
        ///   Writes a Byte Array (max <paramref name="length"/> bytes) to <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="value">
        ///   The value.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <param name="length">
        ///   Length of the data chunk.</param>
        public static void WriteBytes(this Byte[] buffer, Byte[] value, Int32 pos, Int32 length) {

            // ensure value is defined
            if (value == null)
                throw new ArgumentException(nameof(value));

            // validate boundaries - or - clear data chunk (incl. implicit boundary validation in WipeData())
            if (value.Length < length)
                buffer.WipeData(pos, length);
            else
                buffer.CheckDataBoundaries(pos, length);

            // set value
            Buffer.BlockCopy(value, 0, buffer, pos, Math.Min(value.Length, length));

        }

        /// <summary>
        ///   Writes a Byte Array to <paramref name="buffer"/> in reverse order 
        ///   to convert multi-byte arrays between little-endian and big-endian format.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="value">
        ///   The value.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <param name="length">
        ///   Length of the data chunk.</param>
        public static void WriteBytesReverse(this Byte[] buffer, Byte[] value, Int32 pos, Int32 length) {

            // write reverse value (boundary validation happens in WriteBytes())
            buffer.WriteBytes(value.Reverse().ToArray(), pos, length);

        }

        /// <summary>
        ///   Writes a <see cref="UInt16"/> (2 Bytes with big-endian encoding) to <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="value">
        ///   The value.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        public static void WriteUInt16(this Byte[] buffer, UInt16 value, Int32 pos) {

            // write value (boundary validation happens in WriteBytesReverse())
            buffer.WriteBytesReverse(BitConverter.GetBytes(value), pos, 2);

        }

        /// <summary>
        ///   Writes a <see cref="UInt32"/> (4 Bytes with big-endian encoding) to <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="value">
        ///   The value.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        public static void WriteUInt32(this Byte[] buffer, UInt32 value, Int32 pos) {

            // write value (boundary validation happens in WriteBytesReverse())
            buffer.WriteBytesReverse(BitConverter.GetBytes(value), pos, 4);

        }

        /// <summary>
        ///   Writes a <see cref="UInt64"/> (8 Bytes with big-endian encoding) to <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="value">
        ///   The value.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        public static void WriteUInt64(this Byte[] buffer, UInt64 value, Int32 pos) {

            // write value (boundary validation happens in WriteBytesReverse())
            buffer.WriteBytesReverse(BitConverter.GetBytes(value), pos, 8);

        }

        /// <summary>
        ///   Writes a <see cref="String"/> to <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="value">
        ///   The value.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <param name="length">
        ///   Length of the data chunk.</param>
        public static void WriteString(this Byte[] buffer, String value, Int32 pos, Int32 length) {

            // clear data chunk for empty values (incl. boundary validation) - or - copy up to "length"
            // characters of the value into the buffer
            if (String.IsNullOrWhiteSpace(value))
                buffer.WipeData(pos, length);
            else
                buffer.WriteBytes(Encoding.UTF8.GetBytes(value), pos, length);

        }

        /// <summary>
        ///   Writes an Enumeration (1, 2, 4 or 8 byte numbers only) to <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="value">
        ///   The value.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <param name="length">
        ///   Length of the data chunk.</param>
        /// <returns>
        ///   The (mapped or fallback) value.</returns>
        public static void WriteEnum<T>(this Byte[] buffer, T value, Int32 pos, Int32 length) where T : Enum {

            // write value as underlaying type code (boundary validation happens in Write<Type>())
            switch (value.GetTypeCode()) {

                case TypeCode.Byte:
                    if (length == 1)
                        buffer.WriteByte((Byte)(Object)value, pos);
                    break;

                case TypeCode.UInt16:
                    if (length == 2)
                        buffer.WriteUInt16((UInt16)(Object)value, pos);
                    break;

                case TypeCode.UInt32:
                    if (length == 4)
                        buffer.WriteUInt32((UInt32)(Object)value, pos);
                    break;

                case TypeCode.UInt64:
                    if (length == 8)
                        buffer.WriteUInt64((UInt64)(Object)value, pos);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(value));

            }

        }

        /// <summary>
        ///   Writes the date and time to <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <param name="value">
        ///   The value (.net DateTime).</param>
        public static void WriteDateTime(this Byte[] buffer, DateTime value, Int32 pos) {

            // convert and write value (boundary validation happens in Write<Type>())
            buffer.WriteUInt32((UInt32)value.Subtract(Klf200DatagramHelper.UtcMinUnixDateTime).TotalSeconds, pos);

        }

        /// <summary>
        ///   Writes the data of a bit array (little-endian) to <paramref name="buffer"/>.</summary>
        /// <param name="buffer">
        ///   A <see cref="Byte"/> Array.</param>
        /// <param name="pos">
        ///   Start position of the data chunk.</param>
        /// <param name="length">
        ///   Length of the data chunk.</param>
        /// <param name="value">
        ///   The value (.net DateTime).</param>
        public static void WriteBits(this Byte[] buffer, BitArray value, Int32 pos, Int32 length) {

            // clear data chunk for empty values (incl. boundary validation) - or - copy bits from value array
            // up to "length" bytes into the buffer
            if (value == null) {

                buffer.WipeData(pos, length);

            }
            else {

                Byte[] bytes = new Byte[Math.Max(1, value.Length / 8)];
                value.CopyTo(bytes, 0);

                buffer.WriteBytes(bytes, pos, length);

            }

        }

        ///// <summary>
        /////   Converts a 0..51200 value range into 100..0 and maps the unknown value 0xF7FF to Byte.Max.</summary>
        ///// <param name="value">
        /////   A value (0..51200).</param>
        ///// <returns>
        /////   The value (0..100%).</returns>
        //public static Byte RangeToPercentReverse(this UInt16 value) {

        //    if (value == 0xF7FF)
        //        return Byte.MaxValue;
        //    else
        //        return (Byte)(100 - (value / 512));

        //}

    }

}
