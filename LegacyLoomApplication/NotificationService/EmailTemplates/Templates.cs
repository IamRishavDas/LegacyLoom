using System.ComponentModel.DataAnnotations;

namespace NotificationService.EmailTemplates
{
    public enum TemplateName
    {
        WELCOME, OTP_RECOVERY
    }
    public class Templates
    {
        public string GetTemplate([Required]TemplateName templateName, string? userName, string? otp = null)
        {
            List<string> templateContents = new()
            {
                #region WELCOME Email
                $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Welcome to Legacy Loom</title>
                    <style>
                        body {{
                            margin: 0;
                            padding: 0;
                            font-family: 'Georgia', 'Times New Roman', serif;
                            background: #f5f7fa;
                            color: #333;
                        }}
                        .email-container {{
                            max-width: 600px;
                            width: 100%;
                            background: #fff;
                            border-radius: 8px;
                            margin: 20px auto;
                            border: 1px solid #ddd;
                        }}
                        .header {{
                            padding: 30px;
                            text-align: center;
                            background: #8b4513;
                            color: #fff;
                        }}
                        .logo-container {{
                            display: inline-block;
                        }}
                        .brand-name {{
                            font-size: 24px;
                            font-weight: bold;
                            margin-bottom: 5px;
                        }}
                        .tagline {{
                            font-size: 12px;
                            text-transform: uppercase;
                            letter-spacing: 1px;
                            opacity: 0.8;
                        }}
                        .hero-image {{
                            width: 100%;
                            height: 250px;
                            object-fit: cover;
                            display: block;
                        }}
                        .hero-text {{
                            position: absolute;
                            bottom: 15px;
                            left: 20px;
                            color: #fff;
                            text-shadow: 1px 1px 2px rgba(0,0,0,0.5);
                        }}
                        .hero-title {{
                            font-size: 16px;
                            font-weight: bold;
                        }}
                        .hero-subtitle {{
                            font-size: 12px;
                        }}
                        .content {{
                            padding: 30px;
                        }}
                        .main-title {{
                            font-size: 28px;
                            color: #2c1810;
                            text-align: center;
                            margin: 0 0 20px;
                        }}
                        .main-title::after {{
                            content: '';
                            display: block;
                            width: 50px;
                            height: 2px;
                            background: #d2691e;
                            margin: 10px auto;
                        }}
                        .welcome-box {{
                            padding: 20px;
                            border-left: 3px solid #d2691e;
                            background: #f9f9f9;
                            border-radius: 4px;
                            margin-bottom: 20px;
                        }}
                        .welcome-text {{
                            font-size: 16px;
                            line-height: 1.6;
                            color: #333;
                            margin: 0 0 10px;
                        }}
                        .welcome-subtext {{
                            font-size: 14px;
                            line-height: 1.6;
                            color: #555;
                        }}
                        .feature-item {{
                            display: flex;
                            align-items: center;
                            margin-bottom: 15px;
                            padding: 15px;
                            background: #fff;
                            border: 1px solid #eee;
                            border-radius: 4px;
                        }}
                        .feature-icon {{
                            width: 40px;
                            height: 40px;
                            border-radius: 50%;
                            display: flex;
                            align-items: center;
                            justify-content: center;
                            margin-right: 15px;
                            color: #fff;
                            font-size: 18px;
                        }}
                        .feature-icon-1 {{ background: #ff6b6b; }}
                        .feature-icon-2 {{ background: #4ecdc4; }}
                        .feature-icon-3 {{ background: #45b7d1; }}
                        .feature-title {{
                            font-weight: bold;
                            color: #2c1810;
                            margin-bottom: 5px;
                        }}
                        .feature-desc {{
                            color: #666;
                            font-size: 13px;
                        }}
                        .cta-button {{
                            display: inline-block;
                            padding: 12px 30px;
                            font-size: 16px;
                            color: #fff;
                            text-decoration: none;
                            border-radius: 25px;
                            background: #d2691e;
                            font-weight: bold;
                        }}
                        .support-section {{
                            text-align: center;
                            padding: 20px;
                            background: #f9f9f9;
                            border-radius: 4px;
                            margin-top: 20px;
                        }}
                        .support-section strong {{
                            font-size: 14px;
                            color: #2c1810;
                            margin-bottom: 10px;
                            display: block;
                        }}
                        .support-section p {{
                            font-size: 13px;
                            line-height: 1.5;
                            color: #666;
                            margin: 0;
                        }}
                        .support-section a {{
                            color: #d2691e;
                            text-decoration: none;
                            font-weight: bold;
                        }}
                        .footer {{
                            padding: 20px;
                            background: #2c1810;
                            text-align: center;
                            color: #fff;
                        }}
                        .footer div {{
                            font-size: 14px;
                            font-weight: bold;
                            margin-bottom: 5px;
                        }}
                        .footer p {{
                            font-size: 12px;
                            opacity: 0.8;
                            margin: 10px 0 0;
                        }}

                        /* Media Queries */
                        @media only screen and (max-width: 600px) {{
                            .email-container {{ width: 100%; margin: 0; }}
                            .content {{ padding: 20px; }}
                            .header {{ padding: 20px; }}
                            .brand-name {{ font-size: 20px; }}
                            .tagline {{ font-size: 10px; }}
                            .hero-text {{ left: 15px; }}
                            .hero-title {{ font-size: 14px; }}
                            .hero-subtitle {{ font-size: 11px; }}
                            .main-title {{ font-size: 24px; }}
                            .welcome-box {{ padding: 15px; }}
                            .welcome-text {{ font-size: 15px; }}
                            .welcome-subtext {{ font-size: 13px; }}
                            .feature-item {{ flex-direction: column; text-align: center; padding: 10px; }}
                            .feature-icon {{ margin: 0 0 10px; }}
                            .cta-button {{ padding: 10px 20px; font-size: 14px; }}
                            .support-section {{ padding: 15px; }}
                            .footer {{ padding: 15px; }}
                        }}
                        @media only screen and (max-width: 480px) {{
                            .main-title {{ font-size: 20px; }}
                            .welcome-text {{ font-size: 14px; }}
                            .welcome-subtext {{ font-size: 12px; }}
                            .hero-image {{ height: 200px; }}
                            .feature-item {{ padding: 8px; }}
                            .feature-title {{ font-size: 13px; }}
                            .feature-desc {{ font-size: 12px; }}
                        }}
                    </style>
                </head>
                <body>
                    <table width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                        <tr>
                            <td align=""center"">
                                <table class=""email-container"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                                    <!-- Header -->
                                    <tr>
                                        <td class=""header"">
                                            <div class=""logo-container"">
                                                <div class=""brand-name"">Legacy Loom</div>
                                                <div class=""tagline"">Weaving Stories • Crafting Memories</div>
                                            </div>
                                        </td>
                                    </tr>
                                    <!-- Hero Image Section -->
                                    <tr>
                                        <td style=""position: relative;"">
                                            <img class=""hero-image"" src=""https://images.unsplash.com/photo-1528569937393-ee892b976859?q=80&w=2070&auto=format&fit=crop"" alt=""Artisan weaving on traditional loom"">
                                            <div class=""hero-text"">
                                                <div class=""hero-title"">Craftsmanship Meets Digital Innovation</div>
                                                <div class=""hero-subtitle"">Where traditions are preserved and stories come alive</div>
                                            </div>
                                        </td>
                                    </tr>
                                    <!-- Main Content -->
                                    <tr>
                                        <td class=""content"">
                                            <div class=""welcome-box"">
                                                <p class=""welcome-text"">Hello <strong style=""color: #d2691e;"">{userName}</strong>,</p>
                                                <p class=""welcome-subtext"">
                                                    Welcome to Legacy Loom, where digital innovation meets timeless craftsmanship. You're now part of a community that celebrates the art of storytelling through weaving, preserving traditions while creating new memories that will last for generations.
                                                </p>
                                            </div>
                                            <!-- CTA Button -->
                                            <div style=""text-align: center; margin: 30px 0;"">
                                                <a class=""cta-button"" href=""https://legacyloom.netlify.app/"" target=""_blank"">Begin Your Legacy</a>
                                            </div>
                                            <!-- Support Section -->
                                            <div class=""support-section"">
                                                <strong>Need assistance?</strong>
                                                Reach out at <a href=""mailto:legacyloomapp@gmail.com"">legacyloomapp@gmail.com</a></p>
                                            </div>
                                        </td>
                                    </tr>
                                    <!-- Footer -->
                                    <tr>
                                        <td class=""footer"">
                                            <div>Legacy Loom</div>
                                            <p>© 2025 Legacy Loom. All rights reserved.</p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>
                ",
                #endregion

                #region OTP_Email
                $@"
                <!DOCTYPE html>
                <html>
                <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Your Legacy Loom Access Code</title>
                <style>
                    body {{
                        margin: 0;
                        padding: 0;
                        font-family: 'Georgia', 'Times New Roman', serif;
                        background: #f5f7fa;
                        color: #333;
                    }}
                    .email-container {{
                        max-width: 600px;
                        width: 100%;
                        background: #fff;
                        border-radius: 8px;
                        margin: 20px auto;
                        border: 1px solid #ddd;
                        overflow: hidden;
                    }}
                    .header {{
                        padding: 30px;
                        text-align: center;
                        background: #8b4513;
                        color: #fff;
                    }}
                    .logo-container {{
                        display: inline-block;
                    }}
                    .brand-name {{
                        font-size: 24px;
                        font-weight: bold;
                        margin-bottom: 5px;
                    }}
                    .tagline {{
                        font-size: 12px;
                        text-transform: uppercase;
                        letter-spacing: 1px;
                        opacity: 0.8;
                    }}
                    .hero-image {{
                        width: 100%;
                        height: 200px;
                        object-fit: cover;
                        display: block;
                    }}
                    .hero-text {{
                        position: absolute;
                        bottom: 15px;
                        left: 20px;
                        color: #fff;
                        text-shadow: 1px 1px 2px rgba(0,0,0,0.5);
                    }}
                    .hero-title {{
                        font-size: 16px;
                        font-weight: bold;
                    }}
                    .hero-subtitle {{
                        font-size: 12px;
                    }}
                    .content {{
                        padding: 30px;
                    }}
                    .main-title {{
                        font-size: 28px;
                        color: #2c1810;
                        text-align: center;
                        margin: 0 0 20px;
                    }}
                    .main-title::after {{
                        content: '';
                        display: block;
                        width: 50px;
                        height: 2px;
                        background: #d2691e;
                        margin: 10px auto;
                    }}
                    .security-notice {{
                        padding: 20px;
                        border-left: 3px solid #ff6b6b;
                        background: #fff5f5;
                        border-radius: 4px;
                        margin-bottom: 20px;
                    }}
                    .security-text {{
                        font-size: 16px;
                        line-height: 1.6;
                        color: #333;
                        margin: 0 0 10px;
                    }}
                    .security-subtext {{
                        font-size: 14px;
                        line-height: 1.6;
                        color: #555;
                        margin: 0;
                    }}
                    .otp-container {{
                        background: linear-gradient(135deg, #8b4513, #d2691e);
                        border-radius: 8px;
                        padding: 30px;
                        text-align: center;
                        margin: 25px 0;
                        position: relative;
                        overflow: hidden;
                    }}
                    .otp-container::before {{
                        content: '';
                        position: absolute;
                        top: 0;
                        left: 0;
                        right: 0;
                        bottom: 0;
                        background: url('data:image/svg+xml,<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 100 100""><circle cx=""20"" cy=""20"" r=""1"" fill=""rgba(255,255,255,0.1)""/><circle cx=""80"" cy=""40"" r=""1.5"" fill=""rgba(255,255,255,0.1)""/><circle cx=""40"" cy=""80"" r=""1"" fill=""rgba(255,255,255,0.1)""/><circle cx=""90"" cy=""80"" r=""0.8"" fill=""rgba(255,255,255,0.1)""/></svg>');
                        pointer-events: none;
                    }}
                    .otp-label {{
                        color: #fff;
                        font-size: 14px;
                        margin-bottom: 10px;
                        text-transform: uppercase;
                        letter-spacing: 1px;
                        opacity: 0.9;
                    }}
                    .otp-code {{
                        font-size: 36px;
                        font-weight: bold;
                        color: #fff;
                        letter-spacing: 8px;
                        margin: 10px 0;
                        text-shadow: 0 2px 4px rgba(0,0,0,0.3);
                        position: relative;
                        z-index: 1;
                    }}
                    .otp-validity {{
                        color: #fff;
                        font-size: 12px;
                        opacity: 0.8;
                        margin-top: 10px;
                    }}
                    .instructions-box {{
                        background: #f9f9f9;
                        border-radius: 4px;
                        padding: 20px;
                        margin: 20px 0;
                    }}
                    .instructions-title {{
                        font-weight: bold;
                        color: #2c1810;
                        margin-bottom: 10px;
                        font-size: 16px;
                    }}
                    .instructions-list {{
                        margin: 0;
                        padding-left: 20px;
                        color: #666;
                        font-size: 14px;
                        line-height: 1.6;
                    }}
                    .instructions-list li {{
                        margin-bottom: 5px;
                    }}
                    .warning-box {{
                        background: #fff3cd;
                        border: 1px solid #ffeaa7;
                        border-radius: 4px;
                        padding: 15px;
                        margin: 20px 0;
                        display: flex;
                        align-items: center;
                    }}
                    .warning-icon {{
                        width: 24px;
                        height: 24px;
                        background: #f39c12;
                        border-radius: 50%;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        margin-right: 12px;
                        color: #fff;
                        font-weight: bold;
                        font-size: 14px;
                        flex-shrink: 0;
                    }}
                    .warning-text {{
                        font-size: 14px;
                        color: #856404;
                        margin: 0;
                    }}
                    .support-section {{
                        text-align: center;
                        padding: 20px;
                        background: #f9f9f9;
                        border-radius: 4px;
                        margin-top: 20px;
                    }}
                    .support-section strong {{
                        font-size: 14px;
                        color: #2c1810;
                        margin-bottom: 10px;
                        display: block;
                    }}
                    .support-section p {{
                        font-size: 13px;
                        line-height: 1.5;
                        color: #666;
                        margin: 0;
                    }}
                    .support-section a {{
                        color: #d2691e;
                        text-decoration: none;
                        font-weight: bold;
                    }}
                    .footer {{
                        padding: 20px;
                        background: #2c1810;
                        text-align: center;
                        color: #fff;
                    }}
                    .footer div {{
                        font-size: 14px;
                        font-weight: bold;
                        margin-bottom: 5px;
                    }}
                    .footer p {{
                        font-size: 12px;
                        opacity: 0.8;
                        margin: 10px 0 0;
                    }}

                    /* Media Queries */
                    @media only screen and (max-width: 600px) {{
                        .email-container {{ width: 100%; margin: 0; }}
                        .content {{ padding: 20px; }}
                        .header {{ padding: 20px; }}
                        .brand-name {{ font-size: 20px; }}
                        .tagline {{ font-size: 10px; }}
                        .hero-text {{ left: 15px; }}
                        .hero-title {{ font-size: 14px; }}
                        .hero-subtitle {{ font-size: 11px; }}
                        .main-title {{ font-size: 24px; }}
                        .otp-container {{ padding: 20px; }}
                        .otp-code {{ font-size: 28px; letter-spacing: 6px; }}
                        .security-notice {{ padding: 15px; }}
                        .instructions-box {{ padding: 15px; }}
                        .warning-box {{ flex-direction: column; text-align: center; }}
                        .warning-icon {{ margin: 0 0 10px; }}
                        .support-section {{ padding: 15px; }}
                        .footer {{ padding: 15px; }}
                    }}
                    @media only screen and (max-width: 480px) {{
                        .main-title {{ font-size: 20px; }}
                        .hero-image {{ height: 150px; }}
                        .otp-code {{ font-size: 24px; letter-spacing: 4px; }}
                        .instructions-title {{ font-size: 14px; }}
                        .instructions-list {{ font-size: 13px; }}
                    }}
                </style>
                </head>
                <body>
                <table width=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                    <tr>
                        <td align=""center"">
                            <table class=""email-container"" cellspacing=""0"" cellpadding=""0"" border=""0"">
                                <!-- Header -->
                                <tr>
                                    <td class=""header"">
                                        <div class=""logo-container"">
                                            <div class=""brand-name"">Legacy Loom</div>
                                            <div class=""tagline"">Weaving Stories • Crafting Memories</div>
                                        </div>
                                    </td>
                                </tr>
                                <!-- Hero Image Section -->
                                <tr>
                                    <td style=""position: relative;"">
                                        <img class=""hero-image"" src=""https://images.unsplash.com/photo-1617972882867-3707f274c115?q=80&w=1171&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"" alt=""Secure key and lock representing account security"">
                                        <div class=""hero-text"">
                                            <div class=""hero-title"">Secure Access Verification</div>
                                            <div class=""hero-subtitle"">Your account security is our priority</div>
                                        </div>
                                    </td>
                                </tr>
                                <!-- Main Content -->
                                <tr>
                                    <td class=""content"">
                                        <h1 class=""main-title"">Access Code Request</h1>
                            
                                        <div class=""security-notice"">
                                            <p class=""security-text"">Hello <strong style=""color: #d2691e;"">{userName}</strong>,</p>
                                            <p class=""security-subtext"">
                                                We received a request to access your Legacy Loom account. Use the verification code below to continue with your secure login.
                                            </p>
                                        </div>

                                        <!-- OTP Code Container -->
                                        <div class=""otp-container"">
                                            <div class=""otp-label"">Your Verification Code</div>
                                            <div class=""otp-code"">{otp}</div>
                                            <div class=""otp-validity"">Valid for 5 minutes</div>
                                        </div>

                                        <!-- Security Warning -->
                                        <div class=""warning-box"">
                                            <div class=""warning-icon"">!</div>
                                            <p class=""warning-text"">
                                                <strong>Security Notice:</strong> If you didn't request this code, please ignore this email or contact our support team immediately. Never share this code with anyone.
                                            </p>
                                        </div>

                                        <!-- Support Section -->
                                        <div class=""support-section"">
                                            <strong>Need assistance?</strong>
                                            <p>If you're having trouble accessing your account, reach out to us at <a href=""mailto:legacyloomapp@gmail.com"">legacyloomapp@gmail.com</a></p>
                                        </div>
                                    </td>
                                </tr>
                                <!-- Footer -->
                                <tr>
                                    <td class=""footer"">
                                        <div>Legacy Loom</div>
                                        <p>© 2025 Legacy Loom. All rights reserved.</p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                </body>
                </html>
                "
                #endregion
            };
            return templateContents[(int)templateName];
        }
        
    }
}
