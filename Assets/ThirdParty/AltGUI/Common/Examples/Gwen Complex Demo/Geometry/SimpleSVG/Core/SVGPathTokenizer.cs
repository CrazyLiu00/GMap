//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;


namespace Alt.GUI.Demo.SimpleSVG
{
    // SVG path tokenizer. 
    //
    // The tokenizer does all the routine job of parsing the SVG paths.
    // It doesn't recognize any graphical primitives, it even doesn't know
    // anything about pairs of coordinates (X,Y). The purpose of this class 
    // is to tokenize the numeric values and commands. SVG paths can 
    // have single numeric values for Horizontal or Vertical line_to commands 
    // as well as more than two coordinates (4 or 6) for Bezier curves 
    // depending on the semantics of the command.
    // The behaviour is as follows:
    //
    // Each call to next() returns true if there's new command or new numeric
    // value or false when the path ends. How to interpret the result
    // depends on the sematics of the command. For example, command "C" 
    // (cubic Bezier curve) implies 6 floating point numbers preceded by this 
    // command. If the command assumes no arguments (like z or Z) the 
    // the last_number() values won't change, that is, last_number() always
    // returns the last recognized numeric value, so does last_command().

    public class SVGPathTokenizer
    {
        int[] m_separators_mask = new int[256 / 8];
        int[] m_commands_mask = new int[256 / 8];
        int[] m_numeric_mask = new int[256 / 8];

        string m_path = null;
        double m_last_number = 0;
        int m_last_command = 0;

        static string s_commands = "+-MmZzLlHhVvCcSsQqTtAaFfPp";
        static string s_numeric = ".Ee0123456789";
        static string s_separators = " ,\t\n\r";


        public SVGPathTokenizer()
        {
            init_char_mask(m_commands_mask, s_commands);
            init_char_mask(m_numeric_mask, s_numeric);
            init_char_mask(m_separators_mask, s_separators);
        }

        public void set_path_str(string str)
        {
            m_path = str;
            m_last_command = 0;
            m_last_number = 0.0;
        }

        public bool next()
        {
            if (string.IsNullOrEmpty(m_path))// == 0)
                return false;

            // Skip all white spaces and other garbage
            //while(*m_path && !is_command(*m_path) && !is_numeric(*m_path)) 
            while (m_path.Length > 0 &&
                !is_command(m_path[0]) && !is_numeric(m_path[0]))
            {
                if (!is_separator(m_path[0]))
                {
                    //char buf[100];                
                    //sprintf(buf, "path_tokenizer.next : Invalid Character %c", *m_path);
                    //throw new SVGException(new string(buf));
                    throw new SVGException("path_tokenizer.next : Invalid Character " + m_path[0]);
                }

                //m_path++;
                m_path = m_path.Remove(0, 1);
            }

            //if(*m_path == 0)
            if (string.IsNullOrEmpty(m_path))
                return false;

            if (is_command(m_path[0]))
            {
                // Check if the command is a numeric sign character
                if (m_path[0] == '-' || m_path[0] == '+')
                {
                    return parse_number();
                }

                //m_last_command = *m_path++;
                m_last_command = m_path[0];
                m_path = m_path.Remove(0, 1);

                //while(*m_path && is_separator(*m_path))
                while (m_path.Length > 0 &&
                    is_separator(m_path[0]))
                {
                    //m_path++;
                    m_path = m_path.Remove(0, 1);
                }

                //if (*m_path == 0)
                if (string.IsNullOrEmpty(m_path))
                    return true;
            }

            return parse_number();
        }

        public double next(int cmd)
        {
            if (!next())
                throw new SVGException("parse_path: Unexpected end of path");

            if (last_command() != cmd)
            {
                //char buf[100];
                //sprintf(buf, "parse_path: Command %c: bad or missing parameters", cmd);
                //throw new SVGException(new string(buf));
                throw new SVGException("parse_path: Command " + cmd + ": bad or missing parameters");
            }
            return last_number();
        }


        public int last_command() { return m_last_command; }
        public double last_number() { return m_last_number; }



        static void init_char_mask(int[] mask, string char_set)
        {
            //memset(mask, 0, 256/8);

            int char_set_len = char_set.Length;
            int char_set_index = 0;

            //while(*char_set) 
            while (char_set_index < char_set_len)
            {
                //unsigned c = unsigned(char_set[char_set_index++]) & 0xFF;
                int c = char_set[char_set_index++] & 0xFF;
                mask[c >> 3] |= 1 << (c & 7);
            }
        }

        bool contains(int[] mask, int c)
        {
            return (mask[(c >> 3) & (256 / 8 - 1)] & (1 << (c & 7))) != 0;
        }

        bool is_command(int c)
        {
            return contains(m_commands_mask, c);
        }

        bool is_numeric(int c)
        {
            return contains(m_numeric_mask, c);
        }

        bool is_separator(int c)
        {
            return contains(m_separators_mask, c);
        }

        bool parse_number()
        {
            /*char buf[256]; // Should be enough for any number
            char* buf_ptr = buf;

            // Copy all sign characters
            while(buf_ptr < buf+255 && m_path[0] == '-' || m_path[0] == '+')
            {
                *buf_ptr++ = *m_path++;
            }

            // Copy all numeric characters
            while(buf_ptr < buf+255 && is_numeric(*m_path))
            {
                *buf_ptr++ = *m_path++;
            }
            *buf_ptr = 0;
            m_last_number = atof(buf);*/


            string buf = "";

            // Copy all sign characters
            while (m_path.Length > 0 &&
                (m_path[0] == '-' || m_path[0] == '+'))
            {
                buf = m_path[0].ToString();
                m_path = m_path.Remove(0, 1);
            }

            // Copy all numeric characters
            while (m_path.Length > 0 &&
                is_numeric(m_path[0]))
            {
                buf += m_path[0];
                m_path = m_path.Remove(0, 1);
            }
            /*int len = buf.Length;
            while (m_path.Length > 0)
            {
                if (is_numeric(m_path[0]))
                {
                    buf += m_path[0];
                }
                else if (buf.Length != len || m_path[0] != ' ')
                {
                    break;
                }

                m_path = m_path.Remove(0, 1);
            }

            if (string.IsNullOrEmpty(buf))
            {
                return false;
            }*/

            if (string.IsNullOrEmpty(buf))
            {
                return false;
            }

            m_last_number = double.Parse(buf, System.Globalization.CultureInfo.InvariantCulture);

            return true;
        }
    }
}
