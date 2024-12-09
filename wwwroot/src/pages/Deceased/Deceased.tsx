import Header from "../../components/modules/header/Header";
import "./deceased.css";
import "swiper/css";
import "swiper/css/navigation";
import { Swiper, SwiperSlide } from "swiper/react";
import { Navigation } from "swiper/modules";
import { MdOutlineCloseFullscreen } from "react-icons/md";
import { FaPlay } from "react-icons/fa";

const Deceased = () => {
  return (
    <div>
      <Header />
      <main dir="rtl" className="deceased_main">
        <section className="container">
          <div className="details-container">
            <div className="right-detail-container">
              <div className="detail-container">
                <div className="detail-cover">
                  <div className="id">۴۴۵۳۴</div>
                  <div className="cover-contaier">
                    <div className="cover">
                      <img
                        className="cover-image"
                        src="/images/abbas.png"
                        alt=""
                      />
                    </div>
                    <img
                      className="cover-svg"
                      src="/images/cover2.svg"
                      alt=""
                    />
                  </div>
                  <div className="OC-container">
                    <button className="online-pilgrimage">زیارت مجـازی</button>
                    <button className="condolence-message">
                      ارسال پیام تسلیت
                    </button>
                  </div>
                </div>

                <div className="details">
                  <h2 className="name">عباس آسمیه</h2>
                  <div className="detail-item-container">
                    <div className="detail-item">
                      <div className="item-title">تاریخ ولادت :</div>
                      <div className="item-object">۱۳۶۸/۰۵/۱</div>
                      <div className="item-sub">( ۸ماه :۳۶ روز)</div>
                    </div>
                    <div className="detail-item">
                      <div className="item-title">تاریخ وفات :</div>
                      <div className="item-object">۱۳۹۴/۱۰/۲۱</div>
                      <div className="item-sub">( ۸ماه :۳۶ روز)</div>
                    </div>
                    <div className="detail-item">
                      <div className="item-title">نام پدر :</div>
                      <div className="item-object">محمد</div>
                    </div>
                    <div className="detail-item">
                      <div className="item-title">آدرس مزار :</div>
                      <div className="item-object">
                        اصفهان،گلستان شهدا،قطعه ۱، ردیف ۲، شماره ۳
                      </div>
                    </div>
                  </div>
                  <div className="OC-container-mobile">
                    <button className="online-pilgrimage-mobile">
                      زیارت مجـازی
                    </button>
                    <button className="condolence-message-mobile">
                      ارسال پیام تسلیت
                    </button>
                  </div>
                </div>
              </div>

              <div className="recitation">
                <div className="recitation-item-container">
                  <button
                    className="recitation-item"
                    data-src="./voice/ayatolkorsi.mp3"
                  >
                    آیته الکرسی
                  </button>
                  <button
                    className="recitation-item"
                    data-src="./voice/al-omran.mp3"
                  >
                    سوره آل عمران
                  </button>
                  <button
                    className="recitation-item"
                    data-src="./voice/yasin.mp3"
                  >
                    سوره یاسین
                  </button>
                  <button
                    className="recitation-item"
                    data-src="./voice/Al-Fatiha.mp3"
                  >
                    سوره فاتحه
                  </button>
                </div>
                <div className="recitation-details">
                  <div className="audio-player2">
                    <div className="time2" id="time2">
                      00:00
                    </div>
                    <div className="progress-container2">
                      <div className="progress-bar2" id="progress-bar2">
                        <div className="progress2" id="progress2"></div>
                      </div>
                    </div>
                    <button id="play-btn2" className="play-btn2"></button>
                  </div>

                  <audio id="audio2" src=""></audio>
                </div>
              </div>

              <div className="poetry">
                <div className="poetry-verse">
                  <span className="poetry-stanza">
                    کاش بودی تا دلم تنها نبود
                  </span>
                  <span className="poetry-stanza">تا اسیر غصه فردا نبود</span>
                </div>
                <div className="poetry-verse">
                  <span className="poetry-stanza">
                    کاش بودی تا فقط باور کنی
                  </span>
                  <span className="poetry-stanza">
                    بعد تو این زندگی زیبا نبود
                  </span>
                </div>
              </div>
            </div>
            <div className="left-detail-container">
              <div className="reading-the-Quran">
                <div className="reading-container">
                  <div className="reading-title">ختم قرآن صفحه</div>
                  <div className="reading-counter">
                    <span className="reading-counter-item">۴</span>
                    <span className="reading-counter-item">۰</span>
                    <span className="reading-counter-item">۰</span>
                  </div>
                  <button className="Participation_in_finishing_the_Quran">
                    شرکت در ختم قرآن
                  </button>
                </div>
                <div className="number_of_page_left">
                  <div className="number_of_page_left_title">
                    تعداد ختم های انجام شده :
                  </div>
                  <span className="number_of_page_left_counter">۱۲۴</span>
                </div>
              </div>
              <hr />
              <div className="reading_the_salawat_and_fatiha_container">
                <div className="reading_the_salawat_and_fatiha">
                  <div className="reading_the_salawat_and_fatiha_title">
                    ختم صلوات به صورت تکی
                  </div>
                  <button className="reading_the_salawat_and_fatiha_counter">
                    <span>صلوات</span>
                    <span>۴۴</span>
                  </button>
                </div>
                <div className="reading_the_salawat_and_fatiha">
                  <div className="reading_the_salawat_and_fatiha_title">
                    ختم فاتحه به صورت تکی
                  </div>
                  <button className="reading_the_salawat_and_fatiha_counter">
                    <span>فاتحه</span>
                    <span>۳۵</span>
                  </button>
                </div>
              </div>
              <hr />
              <div className="do_salawat_and_fatiha">
                <div className="do_salawat_and_fatiha_title">
                  جهت تفعل تعدادی صلوات و فاتحه از نوار پایین استفاده کنید.
                </div>
                <div className="do_salawat_and_fatiha_container">
                  <span className="do_salawat_and_fatiha_counter_title">
                    تعداد{" "}
                  </span>
                  <div className="do_salawat_and_fatiha_counter_container">
                    <button>
                      <svg
                        width="10"
                        height="6"
                        viewBox="0 0 10 6"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                      >
                        <path
                          d="M1 1L4.29289 4.29289C4.68342 4.68342 5.31658 4.68342 5.70711 4.29289L9 1"
                          stroke="#4C2602"
                          stroke-width="2"
                          stroke-linecap="round"
                        />
                      </svg>
                    </button>
                    <span
                      className="do_salawat_and_fatiha_counter"
                      id="salawat-count"
                    >
                      ۰
                    </span>
                    <button>
                      <svg
                        width="10"
                        height="6"
                        viewBox="0 0 10 6"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                      >
                        <path
                          d="M1 5L4.29289 1.70711C4.68342 1.31658 5.31658 1.31658 5.70711 1.70711L9 5"
                          stroke="#4C2602"
                          stroke-width="2"
                          stroke-linecap="round"
                        />
                      </svg>
                    </button>
                  </div>
                  <button className="do_salawat_and_fatiha_counter_btn">
                    صلوات تفعل می‌کنم
                  </button>
                </div>
                <div className="do_salawat_and_fatiha_container">
                  <span className="do_salawat_and_fatiha_counter_title">
                    تعداد{" "}
                  </span>
                  <div className="do_salawat_and_fatiha_counter_container">
                    <button>
                      <svg
                        width="10"
                        height="6"
                        viewBox="0 0 10 6"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                      >
                        <path
                          d="M1 1L4.29289 4.29289C4.68342 4.68342 5.31658 4.68342 5.70711 4.29289L9 1"
                          stroke="#4C2602"
                          stroke-width="2"
                          stroke-linecap="round"
                        />
                      </svg>
                    </button>
                    <span
                      className="do_salawat_and_fatiha_counter"
                      id="fatiha-count"
                    >
                      ۰
                    </span>
                    <button>
                      <svg
                        width="10"
                        height="6"
                        viewBox="0 0 10 6"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                      >
                        <path
                          d="M1 5L4.29289 1.70711C4.68342 1.31658 5.31658 1.31658 5.70711 1.70711L9 5"
                          stroke="#4C2602"
                          stroke-width="2"
                          stroke-linecap="round"
                        />
                      </svg>
                    </button>
                  </div>
                  <button className="do_salawat_and_fatiha_counter_btn">
                    فاتحه تفعل می‌کنم
                  </button>
                </div>
              </div>
            </div>
          </div>
          <div className="tags-container">
            <div className="tags-items-container">
              <div className="tags-title">برچسب ها :</div>
              <button className="tags-item">شهید</button>
              <button className="tags-item">شهید در راه وطن</button>
              <button className="tags-item">شهید خدمت</button>
              <button className="tags-item">شهید در راه وطن</button>
              <button className="tags-item">شهید در راه وطن</button>
            </div>
          </div>
        </section>
        <section className="information">
          <div className="container">
            <div className="biography-container">
              <h3 className="biography-title">زندگینامه</h3>
              <p className="biography-content">
                عباس آسمیه دهم تیرماه سال ۱۳۶۸ در رجایی شهر کرج، در خانواده‌ای
                مذهبی به دنیا آمد. او دومین فرزند خانواده بود. از کودکی به هیئت
                و حضور در مساجد و ذکر اهل بیت (ع) علاقه داشت. بسیار آرام و خوش
                اخلاق بود. کارهایش را آرام و خاموش انجام می‌داد. همیشه با وضو
                بود و مراقب رفتارش با اطرافیانش بود. از آن جا که به علوم اسلامی
                علاقه‌مند بود به مطالعه کتاب‌های حوزوی علاقه داشت. بعد از گرفتن
                دیپلم در رشته مدیریت بازرگانی وارد دانشگاه قزوین شد. در دوره
                راهنمایی عضو بسیج شد و چهار سال به صورت افتخاری خادم مسجد معصومه
                (س) در محله ۱۳ آبان کرج بود. در سال ۱۳۹۱ با مدرک لیسانس مدیریت
                بازرگانی فارغ التحصیل شد و بعد به دلیل علاقه‌اش به خدمت، وارد
                واحد هوافضای سپاه شد. مدتی بعد به عنوان تیرانداز نمونه انتخاب
                شد. قصد داشت به سوریه اعزام شود تا از حرم حضرت زینب (س) دفاع
                کند. در دی ماه ۱۳۹۴ به سوریه اعزام شد. روز بیست و یکم دی سال
                ۱۳۹۴ پس از حضور در مناطق تحت کنترل گروه تروریستی در منطقه خان
                طومان در نبرد با دشمن تکفیری به شهادت رسید و پیکر مطهرش پس از ۷
                سال شناسایی و به وطن رجعت نمود.
              </p>
            </div>
            <div className="pictures-countainer">
              <h3 className="pictures-title">گالری تصاویر </h3>
              <div className="pictures-inner-container">
                <Swiper
                  dir="rtl"
                  navigation={{
                    nextEl: ".swiper-button-next",
                    prevEl: ".swiper-button-prev",
                  }}
                  breakpoints={{
                    640: {
                      slidesPerView: 2,
                      spaceBetween: 20,
                    },
                    768: {
                      slidesPerView: 3,
                      spaceBetween: 40,
                    },
                  }}
                  modules={[Navigation]}
                  className="mySwiper"
                >
                  <SwiperSlide>
                    <a
                      href="/images/image1.png"
                      data-fancybox="gallery"
                      data-caption="Image 1"
                    >
                      <img
                        className="picture !h-[220px] !object-cover sm:!h-[150px]"
                        src="/images/image1.png"
                        alt="image1"
                      />
                    </a>
                    <div className="hover-image">
                      <button className="hover-image-btn">
                        <MdOutlineCloseFullscreen className="text-2xl" />
                      </button>
                    </div>
                  </SwiperSlide>
                  <SwiperSlide>
                    <a
                      href="/images/image2.png"
                      data-fancybox="gallery"
                      data-caption="Image 2"
                    >
                      <img
                        className="picture !h-[220px] !object-cover sm:!h-[150px]"
                        src="/images/image2.png"
                        alt="image2"
                      />
                    </a>
                    <div className="hover-image">
                      <button className="hover-image-btn">
                        <MdOutlineCloseFullscreen className="text-2xl" />
                      </button>
                    </div>
                  </SwiperSlide>
                  <SwiperSlide>
                    <a
                      href="/images/image2.png"
                      data-fancybox="gallery"
                      data-caption="Image 2"
                    >
                      <img
                        className="picture !h-[220px] !object-cover sm:!h-[150px]"
                        src="/images/image2.png"
                        alt="image2"
                      />
                    </a>
                    <div className="hover-image">
                      <button className="hover-image-btn">
                        <MdOutlineCloseFullscreen className="text-2xl" />
                      </button>
                    </div>
                  </SwiperSlide>
                  <SwiperSlide>
                    <a
                      href="/images/image2.png"
                      data-fancybox="gallery"
                      data-caption="Image 2"
                    >
                      <img
                        className="picture !h-[220px] !object-cover sm:!h-[150px]"
                        src="/images/image2.png"
                        alt="image2"
                      />
                    </a>
                    <div className="hover-image">
                      <button className="hover-image-btn">
                        <MdOutlineCloseFullscreen className="text-2xl" />
                      </button>
                    </div>
                  </SwiperSlide>
                </Swiper>
              </div>
            </div>
            <div className="videos-container">
              <h3 className="videos-title">گالری ویدئو</h3>
              <div className="videos-inner-container">
                <Swiper
                  dir="rtl"
                  navigation={{
                    nextEl: ".swiper-button-next",
                    prevEl: ".swiper-button-prev",
                  }}
                  breakpoints={{
                    640: {
                      slidesPerView: 2,
                      spaceBetween: 20,
                    },
                    768: {
                      slidesPerView: 3,
                      spaceBetween: 40,
                    },
                  }}
                  modules={[Navigation]}
                  className="mySwiper"
                >
                  <SwiperSlide>
                    <video
                      controls
                      className="h-[220px] w-full rounded-lg object-contain"
                      src="/images/b768044b04e3cde64a93773dfce2930d55121669-480p.mp4"
                    ></video>
                  </SwiperSlide>
                  <SwiperSlide>
                    <video
                      controls
                      className="h-[220px] w-full rounded-lg object-contain"
                      src="/images/b768044b04e3cde64a93773dfce2930d55121669-480p.mp4"
                    ></video>
                  </SwiperSlide>
                  <SwiperSlide>
                    <video
                      controls
                      className="h-[220px] w-full rounded-lg object-contain"
                      src="/images/b768044b04e3cde64a93773dfce2930d55121669-480p.mp4"
                    ></video>
                  </SwiperSlide>
                  <SwiperSlide>
                    <video
                      controls
                      className="h-[180px] w-full rounded-lg object-fill"
                      src="/images/b768044b04e3cde64a93773dfce2930d55121669-480p.mp4"
                    ></video>
                  </SwiperSlide>
                </Swiper>
              </div>
            </div>
            <div className="voice-files-container">
              <h3 className="voice-files-title">فایل های صوتی ماندگار</h3>

              <Swiper
                dir="rtl"
                navigation={{
                  nextEl: ".swiper-button-next",
                  prevEl: ".swiper-button-prev",
                }}
                breakpoints={{
                  640: {
                    slidesPerView: 2,
                    spaceBetween: 20,
                  },
                  768: {
                    slidesPerView: 3,
                    spaceBetween: 40,
                  },
                }}
                modules={[Navigation]}
                className="mySwiper mt-8"
              >
                <SwiperSlide>
                  <div className="audio-player">
                    <div className="audio-container">
                      <div className="progress-container">
                        <div className="progress-bar" id="progress-bar">
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 30px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 20px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 35px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 15px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 25px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 30px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 20px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 35px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 15px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 25px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 30px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 20px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 35px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 15px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 25px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 30px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 20px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 35px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 15px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                          <div
                            className="equalizer-bar"
                            style={{
                              height: " 25px",
                              animationPlayState: "paused",
                            }}
                          ></div>
                        </div>
                      </div>
                      <div className="rounded-full border border-gray-400 p-3 text-sm">
                        <FaPlay className="!text-sm" />
                      </div>
                    </div>
                    <div className="audio-details">
                      <div className="audio-title">فایل شماره یکم</div>
                      <div className="time" id="time">
                        00:00
                      </div>
                    </div>
                  </div>
                </SwiperSlide>
              </Swiper>
            </div>
            <footer>
              <h3 className="footer-title">
                یادبود را برای دوستان خود ارسال کنید
              </h3>
              <div className="share-container">
                <div className="qrCode-container">
                  <div className="qrCode">
                    <img
                      className="qrCode-picture"
                      src="/images/qr-create.svg"
                      alt="qrCode"
                    />
                  </div>
                  <div className="qrCode-btns">
                    <a href="#" className="qrCode-btn">
                      دانلود
                    </a>
                    <button className="qrCode-btn">سفارش گیری QrCode</button>
                  </div>
                </div>
                <div className="share-inner-container">
                  <div className="share">
                    <div className="share-title">اشتراک گذاری :</div>
                    <div className="share-links">
                      <a href="#" className="share-item">
                        <svg
                          width="36"
                          height="37"
                          viewBox="0 0 36 37"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            fill-rule="evenodd"
                            clip-rule="evenodd"
                            d="M11.0319 2.23007H24.3175C29.1894 2.23007 33.1747 6.20963 33.1747 11.0811V15.3527C28.8073 17.3158 24.4017 27.0115 17.9804 24.9047C17.4515 25.2806 16.2325 26.8294 16.147 28.0045C13.9234 27.7084 11.36 25.1603 11.6676 22.4124C7.96671 19.7354 11.0229 14.7937 13.9577 12.8047C20.2477 8.5417 28.98 12.208 24.1227 15.2561C21.1691 17.1095 14.8532 18.3338 15.5098 13.7838C13.7773 14.2835 12.6682 17.5141 14.7543 19.1976C12.8218 21.0961 13.1933 24.586 15.259 25.7319C17.348 20.3193 24.619 21.0266 27.5567 14.5653C29.7669 9.70506 26.49 4.16701 19.9384 5.01773C14.9939 5.65986 10.3591 9.8308 8.04125 14.7777C5.68946 19.7967 6.03945 26.5177 10.8686 29.9049C16.5518 33.8911 22.6023 30.2001 26.2669 25.376C28.427 22.5327 30.3143 19.3828 33.1747 17.5635V25.1369C33.1747 30.0083 29.1889 34.0001 24.3175 34.0001H11.0319C6.16044 34.0001 2.17474 30.0144 2.17474 25.143V11.0871C2.17474 6.21568 6.16044 2.22998 11.0319 2.22998V2.23007Z"
                            fill="#8A8275"
                          />
                        </svg>
                      </a>
                      <a href="#" className="share-item">
                        <svg
                          width="36"
                          height="37"
                          viewBox="0 0 36 37"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            d="M33.101 15.9945C33.0038 15.0871 32.8215 14.191 32.5565 13.3178C32.3038 12.4423 31.9687 11.5927 31.5557 10.7804C31.0553 9.78525 30.4497 8.8466 29.7493 7.98047C28.5126 6.45068 26.9948 5.17142 25.2777 4.2116C24.547 3.81066 23.7924 3.44149 23.0023 3.17145C22.2963 2.90675 21.5697 2.70082 20.8299 2.55575C20.0912 2.38101 19.3329 2.33749 18.5821 2.2301H16.823C16.5806 2.27395 16.3306 2.29364 16.0844 2.31747C13.1078 2.61864 10.2839 3.78221 7.9592 5.66539C7.52618 5.99866 7.12524 6.36815 6.73606 6.74557C5.88621 7.60718 5.11578 8.55646 4.48833 9.58899C4.02735 10.3202 3.63512 11.0925 3.31665 11.8961C2.97366 12.7162 2.71035 13.5673 2.53034 14.4377C2.36768 15.1408 2.27618 15.8594 2.21264 16.5743C2.14116 17.7618 2.17706 18.9532 2.36355 20.1246C2.52572 21.1677 2.79573 22.1912 3.16924 23.1786C4.10357 25.646 5.65133 27.8345 7.66639 29.5375C9.68144 31.2406 12.0973 32.4019 14.6859 32.912C17.6522 33.4997 20.7975 33.2017 23.5932 32.0383C24.8125 32.6778 26.0077 33.3647 27.2309 34.0001H27.4812C27.534 33.9226 27.5818 33.8417 27.6242 33.758C27.6915 32.3757 27.656 30.9781 27.648 29.592C29.5066 28.0315 31.0083 26.0379 31.9529 23.7981C32.4688 22.6054 32.8265 21.3503 33.0172 20.0648C33.2256 18.7178 33.2538 17.349 33.101 15.9945ZM24.0269 21.05C23.9207 21.8907 23.6551 22.7034 23.2444 23.4445C22.2001 25.2236 20.413 26.4944 18.463 27.1101C15.7031 27.9997 12.5379 27.4434 10.2387 25.6764C9.59536 25.1363 8.91611 24.5765 8.49929 23.8337C8.82493 23.8855 9.09498 24.0761 9.40061 24.1752C10.8144 24.64 12.4664 24.3859 13.6101 23.4207C14.4123 22.7815 14.9883 21.8519 15.1869 20.8473C15.3813 19.9142 15.2345 18.9491 14.9683 18.0474C14.738 17.182 14.4085 16.3401 14.2376 15.4585C14.0988 14.6325 14.0908 13.7864 14.2535 12.9646C14.4711 11.8979 14.9547 10.9034 15.6593 10.0735C16.9422 8.62795 18.8801 7.76221 20.8219 7.93282C20.8419 7.9487 20.8775 7.97666 20.8934 7.98842C19.9641 8.6953 19.4796 9.97817 19.8688 11.106C20.1271 11.892 20.5916 12.5992 21.0882 13.2584C21.6162 14.0088 22.2039 14.7157 22.7005 15.4941C23.1294 16.221 23.4947 16.9914 23.7448 17.8015C24.0386 18.8538 24.1692 19.9619 24.0269 21.05Z"
                            fill="#8A8275"
                          />
                        </svg>
                      </a>
                      <a href="#" className="share-item">
                        <svg
                          width="37"
                          height="37"
                          viewBox="0 0 37 37"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            fill-rule="evenodd"
                            clip-rule="evenodd"
                            d="M18.115 3.01917C9.77779 3.01917 3.01917 9.77779 3.01917 18.115C3.01917 20.9675 3.81177 23.6392 5.18928 25.917L3.84311 30.4939C3.50258 31.6517 4.57832 32.7275 5.73604 32.3869L10.313 31.0406C12.5908 32.4183 15.2625 33.2108 18.115 33.2108C26.4521 33.2108 33.2108 26.4521 33.2108 18.115C33.2108 9.77779 26.4521 3.01917 18.115 3.01917ZM14.7006 21.5307C17.7538 24.5839 20.6692 24.9863 21.6986 25.0242C23.2643 25.0819 24.7889 23.8861 25.3832 22.4991C25.5289 22.1595 25.4925 21.7508 25.2485 21.4382C24.4223 20.3801 23.3039 19.622 22.2111 18.8674C21.7389 18.5407 21.0896 18.6418 20.7418 19.1045L19.835 20.4859C19.7343 20.6392 19.5343 20.6919 19.3752 20.6008C18.7615 20.25 17.8668 19.6518 17.2231 19.0081C16.5802 18.3651 16.0183 17.5109 15.703 16.9368C15.6201 16.7858 15.666 16.5994 15.8041 16.4967L17.1982 15.4614C17.614 15.1016 17.687 14.4903 17.381 14.0426C16.7035 13.0514 15.9143 11.7917 14.772 10.9585C14.46 10.7309 14.0654 10.7053 13.7395 10.845C12.351 11.4401 11.1494 12.9641 11.2071 14.5327C11.245 15.5621 11.6475 18.4776 14.7006 21.5307Z"
                            fill="#8A8275"
                          />
                        </svg>
                      </a>
                      <a href="#" className="share-item">
                        <svg
                          width="37"
                          height="36"
                          viewBox="0 0 37 36"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            fill-rule="evenodd"
                            clip-rule="evenodd"
                            d="M23.7053 30.9632C22.6309 31.0193 20.9089 31.049 18.5394 31.0523L18.0876 31.0526C15.484 31.0526 13.6114 31.0228 12.4698 30.9632C11.3283 30.9036 10.3369 30.7149 9.49573 30.3971C8.49435 30.0793 7.62315 29.5529 6.88213 28.818C6.14111 28.083 5.59035 27.219 5.22986 26.2258C4.94947 25.3916 4.77924 24.4083 4.71915 23.2761C4.6626 22.2105 4.63267 20.5027 4.62934 18.1527L4.62903 17.7045C4.62903 15.1223 4.65907 13.2651 4.71915 12.1329C4.77924 11.0007 4.94947 10.0175 5.22986 9.1832C5.59035 8.19004 6.14111 7.32599 6.88213 6.59106C7.62315 5.85612 8.49435 5.30988 9.49573 4.95234C10.3369 4.67426 11.3283 4.50542 12.4698 4.44583C13.5442 4.38975 15.2662 4.36005 17.6357 4.35675L18.0876 4.35645C20.6911 4.35645 22.5637 4.38624 23.7053 4.44583C24.8469 4.50542 25.8382 4.67426 26.6794 4.95234C27.6808 5.30988 28.552 5.85612 29.293 6.59106C30.034 7.32599 30.5848 8.19004 30.9453 9.1832C31.2257 10.0175 31.4059 11.0106 31.486 12.1627C31.5214 13.1442 31.5411 14.6978 31.5453 16.8237L31.5461 17.7045C31.5461 20.2867 31.5161 22.1439 31.456 23.2761C31.3959 24.4083 31.2257 25.3916 30.9453 26.2258C30.5848 27.219 30.034 28.083 29.293 28.818C28.552 29.5529 27.6808 30.0992 26.6794 30.4567C25.8382 30.7348 24.8469 30.9036 23.7053 30.9632ZM26.4105 11.6857C26.1101 12.0035 25.7396 12.1624 25.2989 12.1624C24.8583 12.1624 24.4778 12.0035 24.1574 11.6857C23.8369 11.3679 23.6767 10.9905 23.6767 10.5535C23.6767 10.1165 23.8369 9.73912 24.1574 9.42131C24.4778 9.1035 24.8583 8.9446 25.2989 8.9446C25.7396 8.9446 26.1201 9.1035 26.4405 9.42131C26.761 9.73912 26.9212 10.1165 26.9212 10.5535C26.8811 10.9905 26.7109 11.3679 26.4105 11.6857ZM18.091 24.5576C19.3327 24.5576 20.4843 24.2497 21.5458 23.6339C22.6072 23.0182 23.4484 22.1839 24.0692 21.1312C24.6901 20.0784 25.0005 18.9363 25.0005 17.7048C25.0005 16.4732 24.6901 15.3311 24.0692 14.2784C23.4484 13.2256 22.6072 12.3913 21.5458 11.7756C20.4843 11.1598 19.3327 10.852 18.091 10.852C16.8493 10.852 15.6977 11.1598 14.6363 11.7756C13.5748 12.3913 12.7336 13.2256 12.1128 14.2784C11.4919 15.3311 11.1815 16.4732 11.1815 17.7048C11.1815 18.9363 11.4919 20.0784 12.1128 21.1312C12.7336 22.1839 13.5748 23.0182 14.6363 23.6339C15.6977 24.2497 16.8493 24.5576 18.091 24.5576ZM14.8994 20.8636C15.7806 21.7376 16.842 22.1746 18.0837 22.1746C19.3254 22.1746 20.3869 21.7376 21.2681 20.8636C22.1493 19.9896 22.5899 18.9369 22.5899 17.7054C22.5899 16.4738 22.1493 15.4211 21.2681 14.5471C20.3869 13.6731 19.3254 13.2361 18.0837 13.2361C16.842 13.2361 15.7806 13.6731 14.8994 14.5471C14.0181 15.4211 13.5775 16.4738 13.5775 17.7054C13.5775 18.9369 14.0181 19.9896 14.8994 20.8636Z"
                            fill="#8A8275"
                          />
                        </svg>
                      </a>
                      <a href="#" className="share-item">
                        <svg
                          width="37"
                          height="37"
                          viewBox="0 0 37 37"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            d="M14.7628 28.1566L15.1856 21.7702L26.7807 11.3225C27.2941 10.8545 26.675 10.628 25.9956 11.0356L11.6829 20.0793L5.49273 18.1165C4.16412 17.7391 4.14902 16.8181 5.79469 16.1538L29.906 6.85352C31.0081 6.35529 32.065 7.12529 31.6422 8.81625L27.5356 28.1566C27.2488 29.5305 26.4184 29.8627 25.2709 29.2286L19.0204 24.6086L16.0159 27.5225C15.6687 27.8698 15.3818 28.1566 14.7628 28.1566Z"
                            fill="#8A8275"
                          />
                        </svg>
                      </a>
                    </div>
                  </div>
                  <div className="short-link-container">
                    <div className="short-link-title">لینک کوتاه :</div>
                    <div className="short-link-inner-container">
                      <span className="short-link" id="link">
                        https://www.google.com/search?client=firefox
                      </span>
                      <button className="copy-short-link">کپی</button>
                    </div>
                  </div>
                </div>
              </div>
              <div className="map-container">
                <div className="map-innser-container">
                  <iframe
                    className="map"
                    src="https://www.google.com/maps/embed?pb=!1m14!1m12!1m3!1d414279.14432908!2d51.25756521192464!3d35.786285988652196!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!5e0!3m2!1sen!2s!4v1728044104258!5m2!1sen!2s"
                    width="600"
                    height="450"
                    style={{ border: "0" }}
                    loading="lazy"
                  ></iframe>
                  <div className="routing-container">
                    <a href="#" className="routing-item">
                      مسیریابی در بلد
                    </a>
                    <a href="#" className="routing-item">
                      مسیریابی در نشان
                    </a>
                    <a href="#" className="routing-item">
                      مسیریابی در گوگل
                    </a>
                  </div>
                </div>
              </div>
              <p className="footer-description">
                {" "}
                © تمام حقوق مادی و معنوی متعلق به شهدا می باشد.
              </p>
            </footer>
          </div>
        </section>
      </main>
    </div>
  );
};

export default Deceased;
